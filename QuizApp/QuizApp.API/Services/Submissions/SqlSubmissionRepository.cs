using Microsoft.EntityFrameworkCore;
using QuizApp.Models;
using QuizApp.Shared;
using System.Management.Automation;

namespace QuizApp.API.Services.Submissions
{
    public class SqlSubmissionRepository : ISubmissionRepository
    {
        private readonly ApplicationDbContext _context;

        public SqlSubmissionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RepositoryResult<Submission>> AddSubmission(Submission submission, CancellationToken token = default)
        {
            try
            {
                await _context.Submissions.AddAsync(submission, token);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return RepositoryResult<Submission>.Fail("Failed item append");
            }
            return RepositoryResult<Submission>.Success(submission);
        }

        public async Task<RepositoryResult<Submission>> GetSubmission(long id, CancellationToken token = default)
        {
            Submission? sub = await _context.Submissions.FirstOrDefaultAsync(s => s.Id == id,token);
            if (sub is null)
                return RepositoryResult<Submission>.Fail("NO Submission Found");
            return RepositoryResult<Submission>.Success(sub);
        }

        public IAsyncEnumerable<Submission> GetSubmissions(CancellationToken token = default)
        {
            return _context.Submissions.AsAsyncEnumerable();
        }

        public IAsyncEnumerable<Submission> GetSubmissionsByExamId(string examId, CancellationToken token = default)
        {
            return _context.Submissions.Where(s => s.ExamId == examId).AsAsyncEnumerable();
        }

        public IAsyncEnumerable<Submission> GetSubmissionsByName(string submissionPersonName, CancellationToken token = default)
        {
            return _context.Submissions.Where(s => s.UserId == submissionPersonName).AsAsyncEnumerable();
        }
    }
}
