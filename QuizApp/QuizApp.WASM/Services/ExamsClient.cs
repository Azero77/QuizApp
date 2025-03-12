using QuizApp.Models;
using System.Net.Http.Json;

namespace QuizApp.BlazorWASM.Services
{
    public class ExamsClient
    {
        private readonly HttpClient _client;

        public ExamsClient(HttpClient client)
        {
            _client = client;
        }

        public IAsyncEnumerable<Exam?> GetExamsAsync() => _client.GetFromJsonAsAsyncEnumerable<Exam>("Exams") ?? throw new InvalidDataException();
    }
}
