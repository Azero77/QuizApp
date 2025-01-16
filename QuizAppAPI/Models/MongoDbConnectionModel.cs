using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    /*"QuizExamConnectionString": "mongodb://localhost:27017",
      "Database": "ExamQuestions",
      "Collection"  : "Exams"*/
    public class MongoDbConnectionModel
    {
        public string? ConnectionString { get; set; }
        public string? Database { get; set; }
        public string? Collection { get; set; }
    }
}
