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
    public class MathJaxWordFormatter : WordFormatter
    {
        protected override void MathMlSelector(StringBuilder result, IEnumerable<DocumentFormat.OpenXml.Math.OfficeMath> mathElements)
        {
            foreach (var mathElement in mathElements)
            {
                string pattern = @"(_\{[^}]+\})_\{([^}]+)\}";
                string replacement = "_{$1$2}";
                // Convert OfficeMath to LaTeX
                _document.LoadXml(mathElement.OuterXml);
                string latex = MLConverter.Convert(_document.DocumentElement);
                latex = System.Text.RegularExpressions.Regex.Replace(latex, pattern, replacement);
                result.Append($"\\({latex}\\)");
            }
        }
    }
}
