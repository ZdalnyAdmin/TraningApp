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

        /// <summary>
        /// Navigate to create traning template view
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateTemplate()
        {
            return View();
        }

        /// <summary>
        /// Navigate to created tranings view
        /// </summary>
        /// <returns></returns>
        public ActionResult EditTemplate()
        {
            return View();
        }

        /// <summary>
        /// Navigate to description how to create traning
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            return View();
        }

        public ActionResult ActiveTraining(int id)
        {
            var loggedPerson = Person.GetLoggedPerson(User);

            var org2Train = _db.TrainingsInOrganizations
                               .Where(x=>x.OrganizationID == loggedPerson.OrganizationID)
                               .Select(x=>x.TrainingID)
                               .ToList();

            if(org2Train.IndexOf(id) == -1)
            {
                ViewBag.AccessDenied = true;
            }
            else
            {
                ViewBag.AccessDenied = false;
                var training = _db.Trainings.FirstOrDefault(x => x.TrainingID == id);
                var trainingDetails = _db.TrainingDetails.Where(x => x.TrainingID == training.TrainingID).OrderBy(x => x.DisplayNo).ToList();
                training.SetCreateUserName(_db.Users.FirstOrDefault(x => x.Id == training.CreateUserID).DisplayName);
                var questions = _db.TrainingQuestons.Where(x => x.TrainingID == training.TrainingID).OrderBy(x => x.DisplayNo).ToList();

                var result = _db.TrainingResults.FirstOrDefault(x => x.PersonID == loggedPerson.Id && x.TrainingID == training.TrainingID);

                if (result == null)
                {
                    // Temp - should be deleted after adding training list.
                    result = new TrainingResult();
                    result.PersonID = loggedPerson.Id;
                    result.StartDate = DateTime.Now;
                    result.TrainingID = training.TrainingID;
                    _db.TrainingResults.Add(result);
                    _db.SaveChanges();
                    ViewBag.StartDate = result.StartDate;

                    // ViewBag.AccessDenied = true; - this is correct statement result.
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
            var trainingResult = _db.TrainingResults.FirstOrDefault(x => x.PersonID == loggedPerson.Id && x.TrainingID == model.TrainingID);

            if (trainingResult == null)
            {
                return Json(result);
            }

            trainingResult.EndDate = DateTime.Now;
            trainingResult.Rating = 0;

            foreach(var answer in answers)
            {
                int trainingQuestionID = 0;
                int.TryParse(answer.Key, out trainingQuestionID);
                var question = _db.TrainingQuestons.FirstOrDefault(x => x.TrainingQuestionID == trainingQuestionID && x.TrainingID == model.TrainingID);
                question.Answers = _db.TrainingAnswers.Where(x=> x.TrainingQuestionID == question.TrainingQuestionID).ToList();

                switch(question.Type)
                {
                    case AppEngine.Models.DataObject.QuestionType.Multi:
                        var selectedAnswers = getSelectedAnswers(answer.Value);
                        foreach (var selectedAnswer in selectedAnswers)
                        {
                            var multiAns = question.Answers.FirstOrDefault(x => x.TrainingAnswerID == selectedAnswer);
                            trainingResult.Rating += multiAns != null ? int.Parse(multiAns.Score) : 0;
                        }

                        break;

                    case AppEngine.Models.DataObject.QuestionType.Text:
                        var textAns = question.Answers.FirstOrDefault(x => x.Text == answer.Value);
                        trainingResult.Rating += textAns != null ? int.Parse(textAns.Score) : 0;
                        break;

                    default:
                        int trainingAnswerID = 0;
                        int.TryParse(answer.Value, out trainingAnswerID);
                        var singleAns = question.Answers.FirstOrDefault(x => x.TrainingAnswerID == trainingAnswerID);
                        trainingResult.Rating += singleAns != null ? int.Parse(singleAns.Score) : 0;
                        break;
                }
            }

            _db.SaveChanges();
            result.Succeeded = true;

            return Json(result);
        }

        [HttpPost]
        public JsonResult CheckDate(CheckTrainingDateModel model)
        {
            Result result = new Result() { Succeeded = false };

            if(ModelState.IsValid)
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
            var listOfAnswers = selectedAnswers.Split(new string[]{";"}, StringSplitOptions.RemoveEmptyEntries).ToList();
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