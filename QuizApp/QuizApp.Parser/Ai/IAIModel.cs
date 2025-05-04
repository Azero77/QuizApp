using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WordDocumentTableParserProject.Ai.QuizAIModel;

namespace WordDocumentTableParserProject.Ai
{
    public interface IAIModel
    {
        public Task<AIModelResult> GetResponseAsync(string prompt);
        public Task<AIModelResult> GetResponseAsync(string prompt,byte[] fileBytes);
        public IAsyncEnumerable<string> GetStreamingResponseAsync(string prompt);
        public IAsyncEnumerable<string> GetStreamingResponseAsync(string prompt,byte[] fileBytes);
        public Task WriteStreamingResponseAsync(Stream stream, string prompt);
        public Task WriteStreamingResponseAsync(Stream stream, string prompt, byte[] fileBytes);
    }
}
