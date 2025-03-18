using Microsoft.AspNetCore.Mvc;
using QuizApp.API.Services.Submissions;
using QuizApp.Models;

namespace QuizApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubmissionsController : Controller
    {
        private readonly ISubmissionRepository _repo;

        public SubmissionsController(ISubmissionRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public ActionResult<IAsyncEnumerable<Submission>> GetSubmissions()
        {
            IAsyncEnumerable<Submission> submissions = _repo.GetSubmissions();
            if (submissions is null)
                return NotFound();
            return Ok(submissions);
        }

        [HttpGet("name/{name}")]
        public ActionResult<IAsyncEnumerable<Submission>> GetSubmissionsByName(string name)
        {
            IAsyncEnumerable<Submission> submissions = _repo.GetSubmissionsByName(name);
            if (submissions is null)
                return NotFound();
            return Ok(submissions);
        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetSubmissionsById(string id)
        {
            Submission submissions = await _repo.GetSubmission(id);
            if (submissions is null)
                return NotFound();
            return Ok(submissions);
        }

        [HttpGet("exams/{exam_id}")]
        public ActionResult<IAsyncEnumerable<Submission>> GetSubmissionsByExamName(string exam_id)
        {
            IAsyncEnumerable<Submission> submissions = _repo.GetSubmissionsByExamId(exam_id);
            if (submissions is null)
                return NotFound();
            return Ok(submissions);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddSubmissionAsync([FromBody] Submission submission)
        {
            await _repo.AddSubmission(submission);
            return Ok(submission);
        }
    }
}
