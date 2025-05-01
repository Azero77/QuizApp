using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WordDocumentTableParserProject.Selector;
using WordDocumentTableParserProject.Writer;

namespace WordDocumentTableParserProject.WordFileParser
{
    //Adapter For writer and questionParser
    public class WordDocumentParser : IFileParser
    {
        private readonly string _documentPath;

        public IQuestionSelector Selector => _selector;
        IWriter IFileParser.Writer => _writer;

        private readonly IQuestionSelector _selector;
        private readonly IWriter _writer;
        private WordprocessingDocument _document;
        public WordDocumentParser(string documentPath, IWriter writer, IQuestionSelector selector)
        {
            _documentPath = documentPath;
            _writer = writer;
            _selector = selector;
            _document = WordprocessingDocument.Open(_documentPath, false);
        }

        public async Task ParseAsync()
        {
            //Every QUESTION parsed will be writting to a file by the writer
            var questions = _selector?.Process(_document)!;
            await foreach (var question in questions)
            {
                await _writer.WriteAsync(question);
            }

            await _writer.DisposeAsync();
        }
    }
}
