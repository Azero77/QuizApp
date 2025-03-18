using QuizApp.Models;

namespace QuizApp.API.Services.Submissions
{
    public interface ISubmissionRepository
    {
        Task AddSubmission(Submission submission);
        Task<Submission> GetSubmission(string id);
        IAsyncEnumerable<Submission> GetSubmissions();
        IAsyncEnumerable<Submission> GetSubmissionsByName(string submissionPersonName);
        IAsyncEnumerable<Submission> GetSubmissionsByExamId(string examId);
    }
}