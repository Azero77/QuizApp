using MongoDB.Driver;
using QuizApp.Models;
using QuizAppAPI.Contexts;
using System.Runtime.CompilerServices;

namespace QuizAppAPI.Services.ExamQuestions
{
    public class DbExamQuestionsRepository : IExamQuestionsRepository
    {
        private readonly IMongoCollection<Exam> _exams;

        public DbExamQuestionsRepository(ExamQuestionsContext context)
        {
            _exams = context.Exams!;
        }

        public async Task<Exam> GetExam(string examName, CancellationToken token = default)
        {
            return (await _exams.FindAsync(e => e.Name == examName,cancellationToken : token)).Single();
        }

        public async Task<Exam> GetExamById(string id,CancellationToken token = default)
        {
            return (await _exams.FindAsync(e => e.id == id,cancellationToken : token)).Single();
        }

        public async IAsyncEnumerable<Exam> GetExamsAsync([EnumeratorCancellation] CancellationToken token = default)
        {
            var projection = Builders<Exam>.Projection.Include("Id").Include("Name");
            var cursor = await _exams.FindAsync(
                filter: Builders<Exam>.Filter.Empty,
                options: new FindOptions<Exam>() { Projection = projection });
            // Iterate over the cursor asynchronously
            while (await cursor.MoveNextAsync(token))
            {
                foreach (var exam in cursor.Current)
                {
                    // Yield each exam to the caller
                    yield return exam;
                }
            }
        }
       
        public async IAsyncEnumerable<Question> GetQuestionsAsync(string examId, [EnumeratorCancellation] CancellationToken token = default)
        {
            var exam = await GetExamById(examId,token);

            // Iterate over the questions synchronously (since they are in memory)
            foreach (var question in exam.Questions)
            {
                // Yield each question to the caller
                yield return question;
            }
        }
    }
}
