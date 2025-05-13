using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using MongoDB.Driver;
using QuizApp.API;
using QuizApp.API.Services;
using QuizApp.Models;
using QuizAppAPI.Services.ExamQuestions;

namespace QuizAppAPI.Controllers
{
    /// <summary>
    /// API to return the available exams to take
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Authorize]
    [Route("api/[controller]")]
    public class ExamsController : Controller
    {
        private readonly IExamQuestionsRepository _repo;

        public ExamsController(IExamQuestionsRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        [AllowAnonymous]
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
        [Authorize(APIConstants.UserPolicy)]
        public async Task<IActionResult> GetExam(string id,CancellationToken token)
        {
            var result = await _repo.GetExamById(id,token);

            if (result is null)
                return NotFound();

            return Ok(result.Result);
        }

        [HttpGet("{id}/Questions")]
        [ProducesResponseType(typeof(IAsyncEnumerable<Question>), 200)] // Updated response type
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(APIConstants.UserPolicy)]
        public async Task<IActionResult> GetExamQuestions(string id, CancellationToken token)
        {
            var result = _repo.GetQuestionsAsync(id,token); // Use the async method

            if (result is null)
                return NotFound();

            return Ok(result); // Return the IAsyncEnumerable directly
        }


        [HttpPost("add")]
        [Authorize(APIConstants.AdminPolicy)]
        public async Task<IActionResult> AddExamQuestion([FromBody] Exam newExam,CancellationToken token)
        {
            var repositoryResult = await _repo.AddExam(newExam,token);
            if (repositoryResult.IsSuccess)
            {
                return Ok(repositoryResult.Result);
            }
            return BadRequest();
        }

        [HttpPut("update")]
        [Authorize(APIConstants.AdminPolicy)]
        public async Task<IActionResult> UpdateExamQuestion([FromBody] Exam exam,CancellationToken token)
        {
            var repositoryResult = await _repo.UpdateExam(exam, token);
            if (repositoryResult.IsSuccess)
            {
                return Ok(repositoryResult.Result);
            }
            return NotFound();
        }

        [HttpDelete("delete/{id}")]
        [Authorize(APIConstants.AdminPolicy)]
        public async Task<IActionResult> DeleteExam(string id,CancellationToken token)
        {
            RepositoryResult<Exam> repoResult = await _repo.DeleteExam(id,token);
            if (repoResult.IsSuccess)
            {
                return Ok(repoResult.Result);
            }
            return NotFound();
        }
    }
}