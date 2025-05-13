using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.API.Authorization;
using QuizApp.API.Services.Submissions;
using QuizApp.Models;

namespace QuizApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SubmissionsController : Controller
    {
        private readonly ISubmissionRepository _repo;

        public SubmissionsController(ISubmissionRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        [Authorize(APIConstants.AdminPolicy)]
        public ActionResult<IAsyncEnumerable<Submission>> GetSubmissions(CancellationToken token)
        {
            IAsyncEnumerable<Submission> submissions = _repo.GetSubmissions(token);
            if (submissions is null)
                return NotFound();
            return Ok(submissions);
        }

        [HttpGet("name/{name}")]
        [Authorize(APIConstants.UserPolicy)]
        public ActionResult<IAsyncEnumerable<Submission>> GetSubmissionsByName(string name,CancellationToken token)
        {
            IAsyncEnumerable<Submission> submissions = _repo.GetSubmissionsByName(name,token);
            if (submissions is null)
                return NotFound();
            return Ok(submissions);
        }

        [HttpGet("id/{id}")]
        [Authorize(APIConstants.UserPolicy)]
        [OnlyOwnedSubmissionFilter]
        public async Task<IActionResult> GetSubmissionsById(long id, CancellationToken token)
        {
            Submission submissions = await _repo.GetSubmission(id,token);
            if (submissions is null)
                return NotFound();
            return Ok(submissions);
        }

        [HttpGet("exams/{exam_id}")]
        [Authorize(APIConstants.AdminPolicy)]
        public ActionResult<IAsyncEnumerable<Submission>> GetSubmissionsByExamName(string exam_id, CancellationToken token)
        {
            IAsyncEnumerable<Submission> submissions = _repo.GetSubmissionsByExamId(exam_id,token);
            if (submissions is null)
                return NotFound();
            return Ok(submissions);
        }

        [HttpPost("add")]
        [Authorize(APIConstants.UserPolicy)]
        public async Task<IActionResult> AddSubmissionAsync([FromBody] Submission submission, CancellationToken token)
        {
            await _repo.AddSubmission(submission,token);
            return Ok(submission);
        }
    }
}
