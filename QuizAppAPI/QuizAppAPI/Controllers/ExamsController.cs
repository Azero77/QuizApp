using Microsoft.AspNetCore.Mvc;
using Models;
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
        [ProducesResponseType(typeof(IEnumerable<Exam>),200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetExams()
        {
            var result = await _repo.GetExams();
            if (result is null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Exam), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetExam(string id)
        {
            var result = await _repo.GetExamById(id);
            if (result is null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("{id}/Questions")]
        [ProducesResponseType(typeof(IEnumerable<Question>), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetExamQuestions(string id)
        {
            var result = await _repo.GetQuestions(id);
            if (result is null)
                return NotFound();
            return Ok(result);
        }
    }
}
