using Models;

namespace QuizAppAPI.Services.ExamQuestions
{
    public interface IExamQuestionsRepository
    {
        Task<IEnumerable<Question>> GetQuestions(string examId);
        Task<IEnumerable<Exam>> GetExams();
        Task<Exam> GetExam(string examName);
        Task<Exam> GetExamById(string id);
    }
}
