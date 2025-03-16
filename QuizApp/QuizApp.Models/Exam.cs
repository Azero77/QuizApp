using MongoDB.Bson.Serialization.Attributes;

namespace QuizApp.Models
{
    public class Exam
    {

        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? id { get; set; }
        public string? Name { get; set; }
        public List<Question> Questions { get; set; } = new List<Question>();

        public int CorrectExam(Submission sub)
        {
            int result = 0;
            if (sub.Choices.Count() != Questions.Count())
                return 0;
           for (int i = 0; i < sub.Choices.Count(); i++)
            {
                string? choice = sub.Choices[i]?.ToLower();
                if (choice is null)
                    continue;
                if (choice == Questions[i].Answer.ToLower())
                    result++;
            }
            return result;
        }
    }
}
