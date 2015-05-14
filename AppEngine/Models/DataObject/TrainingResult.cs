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
        public float Rating { get; set; }
        public bool? IsPassed { get; set; }
        public float? PossibleRate { get; set; }

        public int? TrainingScore { get; set; }

        public float GetPossibleRate()
        {
                if(Training == null ||
                    Training.Questions == null)
                {
                    return 0;
                }

                float possibleRate = 0;
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
                            float max = 0;

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

        public bool IsCompleted
        {
            get { return EndDate.HasValue; }
        }
    }
}
