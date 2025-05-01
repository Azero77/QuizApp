using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using QuizApp.Models;
using QuizApp.Parser.WordFileParser;
using WordDocumentTableParserProject.WordFileParser;

namespace QuizApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamGeneratorController : Controller
    {
        private readonly IFileParser _parser;

        public ExamGeneratorController(IFileParser parser,IMessager messager)
        {
            _parser = parser;
            messager.Error += OnError;
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
        [RequestSizeLimit(long.MaxValue)]
        public async IAsyncEnumerable<Question> Index(IFormFile file)
        {
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
                await file.CopyToAsync(stream);
            }
            await foreach (var question in _parser.ParseQuestionAsync(filePath))
            {
                yield return question;
            }
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
    }
}
