using AppEngine.Models.Common;
using AppEngine.Models.DataBusiness;
using AppEngine.Models.DataContext;
using AppEngine.Models.ViewModels.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace OrganizationModule.Controllers
{
    [Authorize]
    public class TrainingController : Controller
    {
        #region Private fields
        private EFContext _db = new EFContext();
        #endregion

        public ActionResult TrainingList()
        {
            var loggedPerson = Person.GetLoggedPerson(User);
            var trainings = _db.Trainings
                            .Join(_db.TrainingsInOrganizations
                                     .Where(y => y.OrganizationID == loggedPerson.OrganizationID),
                                  x => x.TrainingID,
                                  y => y.TrainingID,
                                  (x, y) => x)
                            .OrderByDescending(x => x.CreateDate)
                            .ToList();

            var globalTrainings = _db.Trainings.Where(x => x.TrainingType == TrainingType.Kenpro).ToList();

            globalTrainings.ForEach(x =>
            {
                if (trainings.IndexOf(x) == -1)
                {
                    trainings.Add(x);
                }
            });

            ViewBag.Trainings = trainings;
            return View();
        }

        [HttpPost]
        public ActionResult TrainingList(StartTrainingModel model)
        {
            var result = new Result() { Errors = new List<string>() };
            var loggedPerson = Person.GetLoggedPerson(User);

            var trainingInOrg = _db.TrainingsInOrganizations
                                   .FirstOrDefault(x => x.OrganizationID == loggedPerson.OrganizationID && x.TrainingID == model.TrainingID);

            var training = _db.Trainings.FirstOrDefault(x => x.TrainingID == model.TrainingID);

            if (trainingInOrg == null && (training == null || training.TrainingType != TrainingType.Kenpro))
            {
                result.Errors.Add("Nie masz uprawnień by uruchomić to szkolenie");
                return Json(result);
            }

            var rslt = _db.TrainingResults.FirstOrDefault(x => x.PersonID == loggedPerson.Id &&
                                                               x.TrainingID == model.TrainingID &&
                                                               !x.EndDate.HasValue);

            if (rslt != null)
            {
                result.Errors.Add("To szkolenie jest już aktywne :) Znajdziesz je na liście swoich aktywnych szkoleń");
                return Json(result);
            }

            var settings = _db.AppSettings.FirstOrDefault(x => x.IsDefault);

            var activeTrainingsCount = _db.TrainingResults.Where(x => x.PersonID == loggedPerson.Id &&
                                                                     !x.EndDate.HasValue)
                                                          .Count();
            if (settings != null &&
                settings.MaxActiveTrainings <= activeTrainingsCount)
            {
                result.Errors.Add("Masz uruchomioną maksymalną ilość kursów, jeżeli chcesz uruchomić kolejny zakończ wcześniej któryś z aktywowanych kursów");
                return Json(result);
            }

            var trainingResult = new TrainingResult();
            trainingResult.PersonID = loggedPerson.Id;
            trainingResult.StartDate = DateTime.Now;
            trainingResult.TrainingID = model.TrainingID;
            _db.TrainingResults.Add(trainingResult);
            _db.SaveChanges();

            result.Succeeded = true;

            return Json(result);
        }

        public ActionResult ActiveTraining(int id)
        {
            string googleDocViewer = "http://docs.google.com/gview?url={0}&embedded=true";

            var loggedPerson = Person.GetLoggedPerson(User);

            var org2Train = _db.TrainingsInOrganizations
                               .Where(x => x.OrganizationID == loggedPerson.OrganizationID)
                               .Select(x => x.TrainingID)
                               .ToList();

            var trn = _db.Trainings.FirstOrDefault(x => x.TrainingID == id);

            if (org2Train.IndexOf(id) == -1 && (trn == null || trn.TrainingType != TrainingType.Kenpro))
            {
                ViewBag.AccessDenied = true;
            }
            else
            {
                ViewBag.AccessDenied = false;
                var training = _db.Trainings.FirstOrDefault(x => x.TrainingID == id);
                var trainingDetails = _db.TrainingDetails.Where(x => x.TrainingID == training.TrainingID).OrderBy(x => x.DisplayNo).ToList();
                trainingDetails.ForEach(x =>
                {
                    if (x.ResourceType == AppEngine.Models.DataObject.TrainingResource.Presentation && !string.IsNullOrWhiteSpace(x.InternalResource))
                    {
                        x.InternalResource = string.Format(googleDocViewer, Request.Url.Scheme + "://" + Request.Url.Authority + "/" + x.InternalResource.Replace("\\", "/"));
                    }

                    if (x.ResourceType == AppEngine.Models.DataObject.TrainingResource.Video && !string.IsNullOrWhiteSpace(x.InternalResource))
                    {
                        x.InternalResource = Request.Url.Scheme + "://" + Request.Url.Authority + "/" + x.InternalResource.Replace("\\", "/");
                    }
                });


                training.UserName = _db.Users.FirstOrDefault(x => x.Id == training.CreateUserID).DisplayName;
                var questions = _db.TrainingQuestons.Where(x => x.TrainingID == training.TrainingID).OrderBy(x => x.DisplayNo).ToList();

                var result = _db.TrainingResults.FirstOrDefault(x => x.PersonID == loggedPerson.Id && x.TrainingID == training.TrainingID);

                if (result == null)
                {
                    ViewBag.AccessDenied = true;
                }
                else
                {
                    ViewBag.StartDate = result.StartDate;
                }

                questions.ForEach(x =>
                {
                    x.Answers = _db.TrainingAnswers.Where(y => y.TrainingQuestionID == x.TrainingQuestionID).ToList();
                });

                ViewBag.GenerateDate = DateTime.Now;
                ViewBag.Training = training;
                ViewBag.TrainingDetails = trainingDetails;
                ViewBag.Questions = questions;
            }

            return View();
        }

        [HttpPost]
        public ActionResult ActiveTraining(SaveTrainingAnswersModel model)
        {
            var result = new Result();
            var s = new System.Web.Script.Serialization.JavaScriptSerializer();
            var answers = s.Deserialize<Dictionary<string, string>>(model.TrainingAnswers);

            var loggedPerson = Person.GetLoggedPerson(User);
            var trainingResult = _db.TrainingResults.FirstOrDefault(x => x.PersonID == loggedPerson.Id &&
                                                                         x.TrainingID == model.TrainingID &&
                                                                         x.EndDate == null);

            if (trainingResult == null)
            {
                return Json(result);
            }

            trainingResult.EndDate = DateTime.Now;
            trainingResult.Rating = 0;
            trainingResult.Training = _db.Trainings.FirstOrDefault(x => x.TrainingID == trainingResult.TrainingID);
            trainingResult.TrainingScore = model.TrainingRate;

            foreach (var answer in answers)
            {
                int trainingQuestionID = 0;
                int.TryParse(answer.Key, out trainingQuestionID);
                var question = _db.TrainingQuestons.FirstOrDefault(x => x.TrainingQuestionID == trainingQuestionID && x.TrainingID == model.TrainingID);
                question.Answers = _db.TrainingAnswers.Where(x => x.TrainingQuestionID == question.TrainingQuestionID).ToList();

                switch (question.Type)
                {
                    case AppEngine.Models.DataObject.QuestionType.Multi:
                        var selectedAnswers = getSelectedAnswers(answer.Value);

                        var wrongAnswer = false;
                        float value = 0;

                        foreach (var selectedAnswer in selectedAnswers)
                        {
                            var multiAns = question.Answers.FirstOrDefault(x => x.TrainingAnswerID == selectedAnswer);

                            if (multiAns == null || wrongAnswer)
                            {
                                continue;
                            }

                            if (multiAns.Score <= 0)
                            {
                                wrongAnswer = true;
                                value = 0;
                                continue;
                            }

                            value += multiAns.Score;
                        }

                        trainingResult.Rating += value;

                        break;

                    case AppEngine.Models.DataObject.QuestionType.Text:
                        var textAns = question.Answers.FirstOrDefault(x => x.Text == answer.Value);
                        trainingResult.Rating += textAns != null ? textAns.Score : 0;
                        break;

                    default:
                        int trainingAnswerID = 0;
                        int.TryParse(answer.Value, out trainingAnswerID);
                        var singleAns = question.Answers.FirstOrDefault(x => x.TrainingAnswerID == trainingAnswerID);
                        trainingResult.Rating += singleAns != null ? singleAns.Score : 0;
                        break;
                }
            }

            trainingResult.PossibleRate = trainingResult.GetPossibleRate();

            if(trainingResult.PossibleRate != 0)
            {
                var percentageResult = trainingResult.Rating / trainingResult.PossibleRate * 100;
                trainingResult.IsPassed = percentageResult >= trainingResult.Training.PassResult;
            }

            _db.SaveChanges();
            result.Succeeded = true;

            return Json(result);
        }

        [HttpPost]
        public JsonResult CheckDate(CheckTrainingDateModel model)
        {
            Result result = new Result() { Succeeded = false };

            if (ModelState.IsValid)
            {
                var training = _db.Trainings.FirstOrDefault(x => x.TrainingID == model.TrainingID);

                if (training != null)
                {
                    result.Succeeded = !(training.ModifiedDate < model.GenereateDate);
                }
            }

            return Json(result);
        }

        #region Private Fields
        private List<int> getSelectedAnswers(string selectedAnswers)
        {
            var listOfAnswers = selectedAnswers.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var listOfSelectedAnswers = listOfAnswers.Select(x =>
            {
                var answer = x.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                return
                new
                {
                    ID = answer.First(),
                    Value = bool.Parse(answer.Last())
                };
            })
            .Where(x => x.Value)
            .Select(x =>
                    {
                        return int.Parse(x.ID);
                    }
                )
            .ToList();

            return listOfSelectedAnswers;
        }
        #endregion
    }
}