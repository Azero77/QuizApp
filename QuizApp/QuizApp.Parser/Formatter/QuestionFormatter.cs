using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using mxd.Dwml;
using QuizApp.Models;
using QuizApp.Parser;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace WordDocumentTableParserProject.Formatter
{
    internal class QuestionFormatter : IQuestionFormatter
    {
        private readonly XmlDocument _document = new();
        private readonly string pattern = @"(_\{[^}]+\})_\{([^}]+)\}";
        private readonly string replacement = "_{$1$2}";

        public Question Format(RawQuestion rawQuestion)
        {
            var question = new Question
            {
                QuestionText = FormatQuestionText(rawQuestion.QuestionText),
                QuestionChoices = FormatQuestionChoices(rawQuestion.QuestionChoices),
                QuestionAnswer = ExtractAnswer(rawQuestion.QuestionAnswer)
            };

            return question;
        }

        private List<string> FormatQuestionChoices(OpenXmlElement questionChoices)
        {
            List<string> questionSentences = new();
            foreach (Paragraph p in questionChoices.Elements<Paragraph>())
            {
                StringBuilder result = new();
                FormatElement(p, result);
                questionSentences.Add(result.ToString());
            }
            return questionSentences;
        }

        private string FormatQuestionText(OpenXmlElement questionText)
        {
            var result = new StringBuilder();
            foreach (Paragraph paragraph in questionText.Elements<Paragraph>())
            {
                FormatElement(paragraph, result);
            }
            return result.ToString();
        }

        private void FormatElement(OpenXmlElement outerElement, StringBuilder result)
        {
                foreach (OpenXmlElement elem in outerElement.Elements())
                {
                    if (elem is Run run)
                    {
                        //should add use case for images in the future here
                        //we are assuming for now that each run element contains only text
                        result.Append(string.Concat(run.Elements<Text>().Select(t => t.Text)));
                    }
                    else if (elem is DocumentFormat.OpenXml.Math.OfficeMath mathElement) 
                    {
                        result.Append(FormatMath(mathElement));
                    }
                    else if (elem is DocumentFormat.OpenXml.Math.Paragraph mathParagraph)
                    {
                        result.Append(FormatMathParagraph(mathParagraph));
                    }
                }
        }


        // Format mathematical equations into LaTeX
        private string FormatMath(DocumentFormat.OpenXml.Math.OfficeMath oMathElement)
        {
            string latex = LoadMathElement(oMathElement);
            return $"\\({latex}\\)";
        }
        /// <summary>
        /// Formatting MathParagraph
        /// </summary>
        /// <param name="oMathElement"></param>
        /// <returns></returns>
        private string FormatMathParagraph(DocumentFormat.OpenXml.Math.Paragraph oMathElement)
        {
            StringBuilder builder = new();
            foreach (DocumentFormat.OpenXml.Math.OfficeMath mathElem in oMathElement.Elements<DocumentFormat.OpenXml.Math.OfficeMath>())
            {
                string latex = LoadMathElement(mathElem);
                builder.Append(latex);
            }

            return $"\\({builder.ToString()}\\)";
        }

        private string LoadMathElement(OpenXmlElement oMathElement)
        {
            _document.LoadXml(oMathElement.OuterXml);
            string latex = MLConverter.Convert(_document.DocumentElement);
            latex = Regex.Replace(latex, pattern, replacement);
            return latex;
        }

        // Extract the answer from the answer element
        private byte? ExtractAnswer(OpenXmlElement answerElement)
        {
            char choice;
            char.TryParse(answerElement.InnerText, out choice);
            if (char.IsLetter(choice))
            {
                return (byte) (choice - 'A');
            }
            return null;
        }

        // TODO: Implement formatter for images or other complex elements
        private QuestionSentence FormatImage(Drawing drawingElement)
        {
            // Extract image data (e.g., base64 or URL) and return as a QuestionSentence
            return new QuestionSentence
            {
                Text = "ImagePlaceholder", // Replace with actual image data
                QuestionSentenceType = QuestionSentenceType.ImageUrl,
                AltText = "Image description" // Replace with actual alt text
            };
        }
    }
}