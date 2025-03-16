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
            return (await _exams.FindAsync(e => e.Name == examName)).Single();
        }

        public async Task<Exam> GetExamById(string id)
        {
            return (await _exams.FindAsync(e => e.id == id)).Single();
        }

        public async IAsyncEnumerable<Exam> GetExamsAsync()
        {
            var cursor = await _exams.FindAsync(_ => true);

            // Iterate over the cursor asynchronously
            while (await cursor.MoveNextAsync())
            {
                foreach (var exam in cursor.Current)
                {
                    // Yield each exam to the caller
                    yield return exam;
                }
            }
        }

        public async IAsyncEnumerable<Question> GetQuestionsAsync(string examId)
        {
            var exam = await GetExamById(examId);

            // Iterate over the questions synchronously (since they are in memory)
            foreach (var question in exam.Questions)
            {
                // Yield each question to the caller
                yield return question;
            }
        }
    }
}
