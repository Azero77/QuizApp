using MongoDB.Driver;
using QuizApp.Models;
using QuizAppAPI.Contexts;
using QuizAppAPI.Services.ExamQuestions;

namespace QuizApp.API.Services.Submissions
{
    public class DbSubmissionRepository : ISubmissionRepository
    {
        private readonly IMongoCollection<Submission> Submissions;

        public DbSubmissionRepository(ExamQuestionsContext context)
        {
            Submissions = context.Submissions;
        }

        public async IAsyncEnumerable<Submission> GetSubmissions()
        {
            var cursor = await Submissions.FindAsync<Submission>(_ => true);
            while (await cursor.MoveNextAsync())
            {
                foreach (Submission item in cursor.Current)
                {
                    yield return item;
                }
            }
        }

        public async Task<Submission> GetSubmission(string id)
        {
            return await (await Submissions.FindAsync<Submission>(s => s.id == id)).SingleAsync();
        }
        public async IAsyncEnumerable<Submission> GetSubmissions(string submissionPersonName)
        {
            var cursor = await Submissions.FindAsync<Submission>(s => s.SubmissionPersonName == submissionPersonName);
            while (await cursor.MoveNextAsync())
            {
                foreach (Submission item in cursor.Current)
                {
                    yield return item;
                }
            }
        }
        public Task AddSubmission(Submission submission)
        {
            return Submissions.InsertOneAsync(submission);
        }
    }
}
