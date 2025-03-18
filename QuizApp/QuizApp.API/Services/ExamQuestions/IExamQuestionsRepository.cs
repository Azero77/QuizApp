using QuizApp.Models;

namespace QuizAppAPI.Services.ExamQuestions
{
    public interface IExamQuestionsRepository
    {
        Task<Exam> GetExam(string examName, CancellationToken token = default);
        Task<Exam> GetExamById(string id,CancellationToken token = default);
        IAsyncEnumerable<Exam> GetExamsAsync(CancellationToken token = default);
        IAsyncEnumerable<Question> GetQuestionsAsync(string examId, CancellationToken token = default);
    }
}