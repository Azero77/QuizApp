using Newtonsoft.Json;
using QuizApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordDocumentTableParserProject.Writer
{
    /// <summary>
    /// Writing .docx file into .json file question by question
    /// </summary>
    public class JsonFileWriter : IWriter
    {

        private readonly JsonWriter _writer;
        private readonly JsonSerializer _serializer = new() { Formatting = Formatting.Indented };

        public JsonFileWriter(string path)
        {
            _writer = new JsonTextWriter(new StreamWriter(path));
            InitializeWriter();
        }

        public JsonFileWriter(JsonWriter writer)
        {
            _writer = writer;
            InitializeWriter();
        }

        private void InitializeWriter()
        {
            _writer.WriteStartArray();
        }

        public Task WriteAsync(Question question)
        {
            return Task.Run(() =>
            {
                _serializer.Serialize(_writer, question);
            });
        }


        public async ValueTask DisposeAsync()
        {
            _writer.WriteEndArray();
            await _writer.CloseAsync();
        }
    }
}
