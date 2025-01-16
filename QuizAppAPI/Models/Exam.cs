using MongoDB.Bson.Serialization.Attributes;

namespace QuizApp.Models
{
    public class Exam
    {

        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? id { get; set; }
        public string? Name { get; set; }
        public IEnumerable<Question> Questions { get; set; } = new List<Question>();
    }
}
