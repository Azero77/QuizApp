using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mxd.Dwml;
using System.Xml;

namespace WordDocumentTableParserProject
{
    public class WordFormatter : IQuestionFormatter
    {
        protected readonly XmlDocument _document = new();
        public string Format(OpenXmlElement cell, QuestionPart questionPart)
        {
            switch (questionPart)
            {
                case QuestionPart.Choices:
                    return FormatMath(cell);
                case QuestionPart.Text:
                    return FormatMath(cell);
                case QuestionPart.Number:
                    return cell.InnerText.Trim();
                case QuestionPart.Answer:
                    return cell.InnerText.Trim();
                default:
                    throw new InvalidDataException("Question Part is not selected");
            }
        }

        public string Format(OpenXmlElement cell)
        {
            return Format(cell, QuestionPart.Text);
        }

        public IEnumerable<string> FormatMany(OpenXmlElement cell,QuestionPart questionPart,Func<OpenXmlElement,IEnumerable<OpenXmlElement>> selector)
        {
            List<string> strings = new List<string>();
            foreach (var item in selector(cell))
            {
                strings.Add(FormatElementMath(item));
            }
            return strings;
        }

        public string FormatMath(OpenXmlElement cell)
        {
            StringBuilder result = new StringBuilder();

            // Iterate through paragraphs in the cell
            foreach (var paragraph in cell.Elements<Paragraph>())
            {
                // Recursively find all OfficeMath elements within the paragraph
                FormatElementMath(paragraph, result);
            }

            return result.ToString();
        }
        public string FormatElementMath(OpenXmlElement element)
        {
            StringBuilder result = new();
            var mathElements = element.Descendants<DocumentFormat.OpenXml.Math.OfficeMath>();
            MathMlSelector(result, mathElements);
            var text = string.Join("", element.Descendants<Run>().Select(r => r.InnerText));
            result.Append(text);
            return result.ToString();
        }

        public virtual void FormatElementMath(OpenXmlElement elemnth, StringBuilder result)
        {
            var mathElements = elemnth.Descendants<DocumentFormat.OpenXml.Math.OfficeMath>();
            MathMlSelector(result, mathElements);
            var text = string.Join("", elemnth.Descendants<Run>().Select(r => r.InnerText));
            result.Append(text);
        }

        protected virtual void MathMlSelector(StringBuilder result, IEnumerable<DocumentFormat.OpenXml.Math.OfficeMath> mathElements)
        {
            foreach (var mathElement in mathElements)
            {
                // Convert OfficeMath to LaTeX
                _document.LoadXml(mathElement.OuterXml);
                string latex = MLConverter.Convert(_document.DocumentElement);
                result.Append(latex);
            }
        }
    }
}
