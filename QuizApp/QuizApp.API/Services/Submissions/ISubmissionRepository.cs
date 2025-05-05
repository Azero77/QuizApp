using QuizApp.Models;

namespace QuizApp.API.Services.Submissions
{
    public interface ISubmissionRepository
    {
        Task<RepositoryResult<Submission>> AddSubmission(Submission submission, CancellationToken token = default);
        Task<RepositoryResult<Submission>> GetSubmission(string id, CancellationToken token = default);
        IAsyncEnumerable<Submission> GetSubmissions(CancellationToken token = default);
        IAsyncEnumerable<Submission> GetSubmissionsByName(string submissionPersonName, CancellationToken token = default);
        IAsyncEnumerable<Submission> GetSubmissionsByExamId(string examId, CancellationToken token = default);
    }
}