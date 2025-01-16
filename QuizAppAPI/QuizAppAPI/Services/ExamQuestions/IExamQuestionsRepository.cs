using Models;
using QuizAppAPI.Models;

namespace QuizAppAPI.Services.ExamQuestions
{
    public interface IExamQuestionsRepository
    {
        Task<IEnumerable<Question>> GetQuestions(string examName);
        Task<IEnumerable<Exam>> GetExams();
        Task<Exam> GetExam(string examName);
    }
}
