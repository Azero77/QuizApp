using MongoDB.Driver;
using QuizApp.Models;
using QuizAppAPI.Contexts;
using QuizAppAPI.Services.ExamQuestions;
using System.Runtime.CompilerServices;

namespace QuizApp.API.Services.Submissions
{
    public class DbSubmissionRepository : ISubmissionRepository
    {
        private readonly IMongoCollection<Submission> Submissions;

        public DbSubmissionRepository(ExamQuestionsContext context)
        {
            Submissions = context.Submissions;
        }

        public async IAsyncEnumerable<Submission> GetSubmissions([EnumeratorCancellation]CancellationToken token = default)
        {
            var cursor = await Submissions.FindAsync<Submission>(_ => true,cancellationToken : token);
            while (await cursor.MoveNextAsync(token))
            {
                foreach (Submission item in cursor.Current)
                {
                    yield return item;
                }
            }
        }

        public async Task<RepositoryResult<Submission>> GetSubmission(string id, CancellationToken token = default)
        {
            var sub = await (await Submissions.FindAsync<Submission>(s => s.Id == id)).SingleAsync();
            return RepositoryResult<Submission>.Success(sub);
        }
        public async IAsyncEnumerable<Submission> GetSubmissionsByName(string submissionPersonName, [EnumeratorCancellation] CancellationToken token = default)
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
        public async IAsyncEnumerable<Submission> GetSubmissionsByExamId(string examId, [EnumeratorCancellation] CancellationToken token = default)
        {
            var cursor = await Submissions.FindAsync<Submission>(s => s.ExamId == examId);
            while (await cursor.MoveNextAsync())
            {
                foreach (Submission item in cursor.Current)
                {
                    yield return item;
                }
            }
        }
        public async Task<RepositoryResult<Submission>> AddSubmission(Submission submission, CancellationToken token = default)
        {
            await Submissions.InsertOneAsync(submission);
            return RepositoryResult<Submission>.Success(submission);
        }
    }
}
