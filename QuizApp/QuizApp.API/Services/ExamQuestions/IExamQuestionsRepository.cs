using QuizApp.Models;

namespace QuizAppAPI.Services.ExamQuestions
{
    public interface IExamQuestionsRepository
    {
        Task<Exam> GetExam(string examName);
        Task<Exam> GetExamById(string id);
        IAsyncEnumerable<Exam> GetExamsAsync();
        IAsyncEnumerable<Question> GetQuestionsAsync(string examId);
    }
}