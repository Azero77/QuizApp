using QuizApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordDocumentTableParserProject.Selector;
using WordDocumentTableParserProject.Writer;

namespace WordDocumentTableParserProject.WordFileParser
{
    public interface IFileParser
    {
        Task ParseAsync(string documentPath);
        IAsyncEnumerable<Question> ParseQuestionAsync(string documentPath);
        IQuestionSelector Selector { get; }
        IWriter Writer { get; }
    }
}
