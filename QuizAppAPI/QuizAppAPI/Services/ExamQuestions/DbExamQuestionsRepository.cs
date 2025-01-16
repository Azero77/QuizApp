using Models;
using MongoDB.Driver;
using QuizAppAPI.Contexts;
using QuizAppAPI.Models;

namespace QuizAppAPI.Services.ExamQuestions
{
    public class DbExamQuestionsRepository : IExamQuestionsRepository
    {
        private readonly IMongoCollection<Exam> _exams;
        public DbExamQuestionsRepository(ExamQuestionsContext context)
        {
            _exams = context.Exams!;
        }
        public async Task<Exam> GetExam(string examName)
        {
            return (await _exams.FindAsync<Exam>(e => e.Name == examName)).Single();
        }

        public async Task<IEnumerable<Exam>> GetExams()
        {
            return (await _exams.FindAsync(_ => true)).ToEnumerable<Exam>();

        }

        public async Task<IEnumerable<Question>> GetQuestions(string examName)
        {
            var exam = await GetExam(examName);
            return exam.Questions;
        }
    }
}
