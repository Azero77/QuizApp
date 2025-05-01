using DocumentFormat.OpenXml.Packaging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuizApp.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordDocumentTableParserProject.Ai;

namespace WordDocumentTableParserProject.QuestionParsers
{
    /// <summary>
    /// This parser work by reading the document and writing into .json file with the schema similiar to Raw question 
    /// </summary>
    public class AIQuestionParser : IQuestionParser
    {
        private readonly IAIModel _model;
        private readonly JsonSerializer _serializer = new() { Formatting = Formatting.Indented };

        public AIQuestionParser(IAIModel model)
        {
            _model = model;
        }

        public void Dispose()
        {
        }

        public async IAsyncEnumerable<RawQuestion> ProcessDocument(WordprocessingDocument document)
        {
            string path = await Convert(document);
            //reading json objects that represents a raw question
            StreamReader text_reader = new(path);
            JsonTextReader jsonReader = new(text_reader) { SupportMultipleContent = false};
            //Deserializing the object
            if ((await jsonReader.ReadAsync()) && jsonReader.TokenType == JsonToken.StartArray)
            {
                if (jsonReader.TokenType == JsonToken.EndArray)
                {
                    DeleteFile(path);
                    Dispose();
                    yield break;
                }

                RawQuestion? rawQuestion = _serializer.Deserialize<RawQuestion>(jsonReader);
                if (rawQuestion is null)
                    yield break;
                yield return rawQuestion;
            }
            yield break;
        }

        private void DeleteFile(string path)
        {
        }

        protected virtual async Task<string> Convert(WordprocessingDocument document)
        {
            //Reading the file
            string prompt = await CreatePrompt(document);
            var stream = CreateFile(out string path);
            await _model.WriteStreamingResponseAsync(stream, prompt);
            return path;
        }

        private static async Task<string> CreatePrompt(WordprocessingDocument document)
        {
            string document_file = await Read(document);
            string questionPrompt = GetPrompt();
            return questionPrompt + document_file;
        }

        private FileStream CreateFile(out string path)
        {
            string fileName = DateTime.Now.ToString("dd-MM-yyyy") + "-"+ Guid.NewGuid().ToString() + ".json";
            path = Directory.GetCurrentDirectory() + "\\" + fileName;
            return File.Create(path);
        }

        private static Task<string> Read(WordprocessingDocument document)
        {
            var reader = new StreamReader(document?.MainDocumentPart?.GetStream() ?? throw new InvalidDataException());
            return reader.ReadToEndAsync();
        }
        private static string GetPrompt()
        {
            return @"You are an AI assistant that extracts structured multiple choice questions from a .docx file that uses Open XML formatting (WordprocessingML). 
        The document contains a series of questions in either tables or paragraphs, formatted with answer choices and correct answers.

            Read the .docx file and output a JSON array.
Each JSON object represents one multiple choice question, containing the original Open XML fragments of the question, choices, and correct answer.
            Output format:
            ----
             [
  {
    ""QuestionText"": ""<w:p>...</w:p>"",      // Open XML for question (can be <w:p> or <w:tc> or even multiple <w:p>)
    ""QuestionChoices"": [
      ""<w:r>...</w:r>"",                    // Open XML for each choice (can be <w:p> or <w:tc> or even multiple <w:p> or <w:r> or anything)
      ""<w:r>...</w:r>"",
      ""<w:r>...</w:r>""
    ],
    ""Answer"": ""<w:r>...</w:r>""             // Open XML for correct choice
  },
  ...
]
            ----
            some instructions to keep in mind while parsing the .docx file:
            - i want each open xml to have the full information about the question part talked about,
            - Preserve the full Open XML element for each part (e.g., <w:p>, <w:r>, <w:tc>), including all child tags, formatting, styles, and math if present.

            - If questions are inside tables, extract the relevant <w:tc> or <w:tr> that contains the content.

            - If questions are in paragraphs, extract the full <w:p> or set of <w:r>s.

            - Include the correct answer's XML, not just the text or index.

            - Each field in the JSON should include the raw XML string that represents the part of the question.

            - Do not remove any nested tags; preserve formatting.

            - Keep the JSON clean and properly escaped.
            - each <o:math> and <o:paramath> keep it as it is and i will handle the conversion later


            here is the file:
            
            ";
            
        }
    }
}
