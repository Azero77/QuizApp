using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp.Models
{
    public class ExamMarkdown
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? id { get; set; } = null!;
        public IEnumerable<string> QuestionChoices { get; set; } = null!;
        public string QuestionText { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
    }
}
