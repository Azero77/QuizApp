using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.API.Services.Submissions;
using QuizApp.Models;

namespace QuizApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(APIConstants.AdminPolicy)]
    public class SubmissionsController : Controller
    {
        private readonly ISubmissionRepository _repo;

        public SubmissionsController(ISubmissionRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public ActionResult<IAsyncEnumerable<Submission>> GetSubmissions(CancellationToken token)
        {
            IAsyncEnumerable<Submission> submissions = _repo.GetSubmissions(token);
            if (submissions is null)
                return NotFound();
            return Ok(submissions);
        }

        [HttpGet("name/{name}")]
        public ActionResult<IAsyncEnumerable<Submission>> GetSubmissionsByName(string name,CancellationToken token)
        {
            IAsyncEnumerable<Submission> submissions = _repo.GetSubmissionsByName(name,token);
            if (submissions is null)
                return NotFound();
            return Ok(submissions);
        }

        [HttpGet("id/{id}")]
        [Authorize(APIConstants.UserPolicy)]
        public async Task<IActionResult> GetSubmissionsById(string id, CancellationToken token)
        {
            Submission submissions = await _repo.GetSubmission(id,token);
            if (submissions is null)
                return NotFound();
            return Ok(submissions);
        }

        [HttpGet("exams/{exam_id}")]
        public ActionResult<IAsyncEnumerable<Submission>> GetSubmissionsByExamName(string exam_id, CancellationToken token)
        {
            IAsyncEnumerable<Submission> submissions = _repo.GetSubmissionsByExamId(exam_id,token);
            if (submissions is null)
                return NotFound();
            return Ok(submissions);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddSubmissionAsync([FromBody] Submission submission, CancellationToken token)
        {
            await _repo.AddSubmission(submission,token);
            return Ok(submission);
        }
    }
}
