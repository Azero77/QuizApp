using MongoDB.Driver;
using QuizApp.Models;
using QuizAppAPI.Contexts;

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

        public async Task<Exam> GetExamById(string id)
        {
            return (await _exams.FindAsync(e => e.id == id)).Single();
        }

        public async Task<IEnumerable<Exam>> GetExams()
        {
            return await (await _exams.FindAsync(_ => true)).ToListAsync();
        }


        public async Task<IEnumerable<Question>> GetQuestions(string examId)
        {
            return (await GetExamById(examId)).Questions;
        }
    }
}
