using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using QuizApp.Parser;
using QuizApp.Parser.WordFileParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace WordDocumentTableParserProject.QuestionParsers
{
    /// <summary>
    /// Parser For word document (espicially) for anas zaggar doucment chemistry 
    /// </summary>
    public class AnasQuestionParser : IQuestionParser
    {
        private WordprocessingDocument _document = null!;
        private Table _table = null!;
        private readonly IMessager _messager;

        public AnasQuestionParser(IMessager messager)
        {
            _messager = messager;
        }

        private IEnumerator<TableRow> _enumerator = null!;
        private IEnumerable<TableRow> RowGenerator()
        {

            IEnumerable<TableRow>? rows = _table?.Elements<TableRow>();
            if (rows is null)
            {
                _messager.Message("No Rows Are Added");
                yield break;
            }
            foreach (var row in rows)
            {
                yield return row;
            }
        }
        public async IAsyncEnumerable<RawQuestion> ProcessDocument(WordprocessingDocument document)
        {
            AssignFields(document);
            TableRow evenRow, oddRow;
            while (true)
            {
                if (!_enumerator.MoveNext()) yield break; // No more rows to process
                oddRow = _enumerator.Current;

                if (!_enumerator.MoveNext())
                { 
                    _messager.Message("Document must have an even number of rows for questions and answers.");
                    yield break;
                }

                evenRow = _enumerator.Current;

                yield return ProcessQuestion(evenRow, oddRow);
            }
        }

        private void AssignFields(WordprocessingDocument document)
        {
            _document = document;
            AssignTable();
            _enumerator = RowGenerator().GetEnumerator();
        }

        private void AssignTable()
        {
            IEnumerable<Table>? tables = _document?.MainDocumentPart?.Document?.Body?.Elements<Table>();
            if (tables is null)
            {
                _messager.Message("Invalid Document");
                return;
            }
            else if (tables.Count() != 1)
            {
                _messager.Message("Document Contains zero or More than one table please select the current table by index or name");
                return;
            }
            _table = tables.Single();
        }
        protected RawQuestion ProcessQuestion(TableRow evenRow, TableRow oddRow)
        {
            var oddCells = oddRow.Elements<TableCell>();
            var evenCells = evenRow.Elements<TableCell>();

            if (oddCells.Count() == 3 && evenCells.Count() == 3)
            {
                var x = new RawQuestion()
                {
                    QuestionText = oddCells.ElementAt(1),
                    QuestionAnswer = evenCells.ElementAt(2),
                    QuestionChoices = evenCells.ElementAt(1)
                };
                return x;

            }

            _messager.Message("Must Have Three columns for each row");
            return null!;
        }
        public void Dispose()
        {
            _document?.Dispose();
        }
    }
}
