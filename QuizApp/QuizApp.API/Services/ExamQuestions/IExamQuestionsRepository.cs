using QuizApp.API.Services;
using QuizApp.Models;

namespace QuizAppAPI.Services.ExamQuestions
{
    public interface IExamQuestionsRepository
    {
        Task<RepositoryResult<Exam>> GetExam(string examName, CancellationToken token = default);
        Task<RepositoryResult<Exam>> GetExamById(string id,CancellationToken token = default);
        IAsyncEnumerable<Exam> GetExamsAsync(CancellationToken token = default);
        IAsyncEnumerable<Question> GetQuestionsAsync(string examId, CancellationToken token = default);

        Task<RepositoryResult<Exam>> AddExam(Exam exam, CancellationToken token = default);
        Task<RepositoryResult<Exam>> UpdateExam(Exam exam, CancellationToken token = default);
        Task<RepositoryResult<Exam>> DeleteExam(Exam exam, CancellationToken token = default);
    }
}
