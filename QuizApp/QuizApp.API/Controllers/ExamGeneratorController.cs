using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using QuizApp.Models;
using WordDocumentTableParserProject.WordFileParser;

namespace QuizApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamGeneratorController : Controller
    {
        private readonly IFileParser _parser;

        public ExamGeneratorController(IFileParser parser)
        {
            _parser = parser;
        }

        /// <summary>
        /// Generating Exam
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async IAsyncEnumerable<Question> Index(IFormFile file)
        {
            string fileExtensions = Path.GetExtension(file.FileName);
            if (fileExtensions != ".docx")
            {
                ModelState.AddModelError("","File should be .docx file");
                yield break;
            }
            string filename = Guid.NewGuid().ToString() + ".docx";
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), $"temp/{filename}");
            System.IO.File.Create(filePath);
            await foreach (var question in _parser.ParseQuestionAsync(filePath))
            {
                yield return question;
            }
            System.IO.File.Delete(filePath);
        }
    }
}
