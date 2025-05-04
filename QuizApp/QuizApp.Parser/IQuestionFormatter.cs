using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordDocumentTableParserProject
{
    /// <summary>
    /// Formatter for every sentence in the word document to format specific on some arguments
    /// </summary>
    public interface IQuestionFormatter
    {
        string Format(OpenXmlElement cell,QuestionPart questionPart);
        string Format(OpenXmlElement cell);
        IEnumerable<string> FormatMany(OpenXmlElement cell, QuestionPart questionPart, Func<OpenXmlElement, IEnumerable<OpenXmlElement>> selector);
    }

    public enum QuestionPart
    {
        Number,
        Text,
        Choices,
        Answer
    }

}
