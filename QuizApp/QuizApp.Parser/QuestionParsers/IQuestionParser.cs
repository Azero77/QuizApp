using DocumentFormat.OpenXml.Packaging;
using QuizApp.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordDocumentTableParserProject.QuestionParsers;

public interface IQuestionParser : IDisposable
{
    IAsyncEnumerable<RawQuestion> ProcessDocument(WordprocessingDocument document);
}
