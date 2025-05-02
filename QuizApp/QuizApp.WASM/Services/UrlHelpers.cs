using Microsoft.AspNetCore.WebUtilities;
using QuizApp.Models;

namespace QuizApp.BlazorWASM.Services
{
    public static class UrlHelpers
    {
        public static string GetSubmissionLink(string examId,string submissionId)
        {
            IDictionary<string, string> queryParams = new Dictionary<string, string>
                {
                    {"SubmissionId",submissionId},
                    {"ExamId",examId}
                };
            string url = QueryHelpers.AddQueryString("/CorrectedExam", queryParams);
            return url;
        }
    }
}
