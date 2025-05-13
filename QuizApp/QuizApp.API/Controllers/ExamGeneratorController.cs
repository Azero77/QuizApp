using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using QuizApp.Models;
using QuizApp.Parser.WordFileParser;
using WordDocumentTableParserProject.WordFileParser;

namespace QuizApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(APIConstants.AdminPolicy)]
    public class ExamGeneratorController : Controller
    {
        private readonly IFileParser _parser;
        private readonly ILogger<ExamGeneratorController> _logger;
        public ExamGeneratorController(IFileParser parser, IMessager messager, ILogger<ExamGeneratorController> logger)
        {
            _parser = parser;
            messager.Error += OnError;
            _logger = logger;
        }

        private void OnError(string error)
        {
            ModelState.AddModelError("", error);
        }

        /// <summary>
        /// Generating Exam
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [RequestSizeLimit(1024 * 1024 * 10)] //10 megabytes limit
        public async IAsyncEnumerable<Question> Index(IFormFile file)
        {
            _logger.LogDebug("Starting Endpoint...");
            string fileExtensions = Path.GetExtension(file.FileName);
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("", "Please upload a file");
                yield break;
            }
            if (fileExtensions != ".docx")
            {
                ModelState.AddModelError("","File should be .docx file");
                yield break;
            }

            string filename = Guid.NewGuid().ToString() + ".docx";
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), $"temp/{filename}");
            await using (var stream = new FileStream(filePath,FileMode.Create))
            {
                _logger.LogDebug($"{filename} is created...");
                await file.CopyToAsync(stream);
            }
            await foreach (var question in _parser.ParseQuestionAsync(filePath))
            {
                _logger.LogDebug("Question Rendered");
                yield return question;
            }
            if (System.IO.File.Exists(filePath))
            {
                _logger.LogDebug($"{filename} is deleted...");
                System.IO.File.Delete(filePath);
            }
        }
    }
}
