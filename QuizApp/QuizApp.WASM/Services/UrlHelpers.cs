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
                    {"submissionId",submissionId},
                    {"examId",examId}
                };
            string url = QueryHelpers.AddQueryString("/submissionResult", queryParams);
            return url;
        }
    }
}
