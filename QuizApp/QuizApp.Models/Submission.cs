using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp.Models
{
    public class Submission
    {
        public string id { get; set; } = string.Empty;
        public string SubmissionPersonName { get; set; } = string.Empty;
        public string ExamId { get; set; } = null!;
        public List<string?> Choices { get; set; } = null!;
    }
}
