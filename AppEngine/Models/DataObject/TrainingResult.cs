using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppEngine.Models.Common
{
    public class TrainingResult
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)] 
        public int TrainingResultID { get; set; }

        public int TrainingID { get; set; }
        public Training Training { get; set; }
        public string PersonID { get; set; }
        public Person Person { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Rating { get; set; }

        [NotMapped]
        public int PossibleRating
        {
            get
            {
                if(Training == null ||
                    Training.Questions == null)
                {
                    return 0;
                }

                var possibleRate = 0;
                Training.Questions.ForEach(x => 
                    {
                        if(x.Answers == null)
                            return;

                        if (x.Type == DataObject.QuestionType.Single)
                        {
                            x.Answers.ForEach(y =>
                            {
                                possibleRate += y.Score;
                            });
                        }
                        else
                        {
                            var max = 0;

                            x.Answers.ForEach(y =>
                            {
                                if(y.Score > max)
                                {
                                    max = y.Score;
                                }
                            });

                            possibleRate += max;
                        }
                    });

                return possibleRate;
            }
        }
        public bool IsCompleted
        {
            get { return EndDate.HasValue; }
        }
    }
}
