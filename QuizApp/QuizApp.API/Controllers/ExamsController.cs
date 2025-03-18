using Microsoft.AspNetCore.Mvc;
using QuizApp.Models;
using QuizAppAPI.Services.ExamQuestions;

namespace QuizAppAPI.Controllers
{
    /// <summary>
    /// API to return the available exams to take
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ExamsController : Controller
    {
        private readonly IExamQuestionsRepository _repo;

        public ExamsController(IExamQuestionsRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IAsyncEnumerable<Exam>), 200)] // Updated response type
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetExams(CancellationToken token)
        {
            var result = _repo.GetExamsAsync(token); // Use the async method

            if (result is null)
                return NotFound();

            return Ok(result); // Return the IAsyncEnumerable directly
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Exam), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetExam(string id,CancellationToken token)
        {
            var result = await _repo.GetExamById(id,token);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("{id}/Questions")]
        [ProducesResponseType(typeof(IAsyncEnumerable<Question>), 200)] // Updated response type
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetExamQuestions(string id, CancellationToken token)
        {
            var result = _repo.GetQuestionsAsync(id,token); // Use the async method

            if (result is null)
                return NotFound();

            return Ok(result); // Return the IAsyncEnumerable directly
        }
    }
}