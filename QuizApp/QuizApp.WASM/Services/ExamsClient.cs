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

        public Task<Exam?> GetExamAsync(string id) => _client.GetFromJsonAsync<Exam>($"Exams/{id}");

        public Task<HttpResponseMessage> SubmitExam(Submission submission)
        {
            return _client.PostAsJsonAsync<Submission>($"Submissions/add",submission);
        }

        public Task<Submission?> GetSubmission(string subId)
        {
            return _client.GetFromJsonAsync<Submission>($"Submissions/id/{subId}");
        }
    }
}
