using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp.Models
{
    public class Submission
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? id { get; set; }
        public string SubmissionPersonName { get; set; } = string.Empty;
        public string ExamId { get; set; } = null!;
        public List<string?> Choices { get; set; } = null!;

        public DateTime DateSubmitted { get; set; }
        public int Result { get; set; }
    }
}
