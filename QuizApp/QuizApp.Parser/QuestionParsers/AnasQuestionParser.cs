using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using QuizApp.Parser;
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
        private IEnumerator<TableRow> _enumerator = null!;
        private IEnumerable<TableRow> RowGenerator()
        {

            IEnumerable<TableRow> rows = _table?.Elements<TableRow>() ?? throw new InvalidDataException("No Rows Are Added");
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
                    throw new InvalidDataException("Document must have an even number of rows for questions and answers.");

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
            IEnumerable<Table> tables = _document?.MainDocumentPart?.Document?.Body?.Elements<Table>() ?? throw new InvalidDataException("Invalid Document");
            if (tables.Count() != 1)
                throw new InvalidDataException("Document Contains zero or More than one table please select the current table by index or name");
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

            throw new InvalidDataException("Must Have Three columns for each row");
        }
        public void Dispose()
        {
            _document?.Dispose();
        }
    }
}
