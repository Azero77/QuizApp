using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using QuizApp.Models;
using QuizApp.Parser.WordFileParser;
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
        public IQuestionSelector Selector => _selector;
        private readonly IMessager _messager;
        IWriter IFileParser.Writer => _writer!;

        private readonly IQuestionSelector _selector;
        private readonly IWriter? _writer;
        public WordDocumentParser(IQuestionSelector selector,
            IMessager messager,
            IWriter? writer = null)
        {
            _writer = writer;
            _messager = messager;
            _selector = selector;
        }

        public async Task ParseAsync(string documentPath)
        {
            WordprocessingDocument document = WordprocessingDocument.Open(documentPath, false);
            //Every QUESTION parsed will be writting to a file by the writer
            if (_writer is null)
            {
                _messager.Message("Writer is not sed");
                return;
            }
            var questions = _selector?.Process(document)!;
            await foreach (var question in questions)
            {
                await _writer.WriteAsync(question);
            }

            await _writer.DisposeAsync();
        }

        public IAsyncEnumerable<Question> ParseQuestionAsync(string documentPath)
        {
            WordprocessingDocument document = WordprocessingDocument.Open(documentPath, false);
            //Every QUESTION parsed will be writting to a file by the writer
            return _selector?.Process(document)!;
        }
    }
}
