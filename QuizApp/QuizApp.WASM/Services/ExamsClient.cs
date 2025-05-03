using QuizApp.BlazorWASM.Pages;
using QuizApp.Models;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QuizApp.BlazorWASM.Services
{
    //methods of the client will silently fail but the delegating handler will take care for dealing with them
    public class ExamsClient
    {
        private readonly HttpClient _client;
        private JsonSerializerOptions _opts = new() { PropertyNameCaseInsensitive = true};
        public ExamsClient(HttpClient client)
        {
            _client = client;
        }

        public async IAsyncEnumerable<Exam?> GetExamsAsync()
        {
            HttpResponseMessage response = await _client.GetAsync("Exams");
            if (response.IsSuccessStatusCode)
            {
                //read the response
                Stream contentStream = await response.Content.ReadAsStreamAsync();
                await foreach (Exam? exam in JsonSerializer.DeserializeAsyncEnumerable<Exam>(contentStream,_opts))
                {
                    yield return exam;
                }
            }
            yield break;
        }

        public async Task<Exam?> GetExamAsync(string id)
        {
            var response = await _client.GetAsync($"Exams/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Exam?>();
            }
            return null;
        }

        public Task<HttpResponseMessage> SubmitExam(Submission submission)
        {
            return _client.PostAsJsonAsync<Submission>($"Submissions/add",submission);
        }

        public async Task<Submission?> GetSubmission(string subId)
        {
            if (subId is null)
                throw new InvalidDataException("Submission Id must be given");
            var response = await _client.GetAsync($"Submissions/id/{subId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Submission?>();
            }
            return null;
        }

        public async IAsyncEnumerable<Submission?> GetSubmissionsByExamId(string examId)
        {
            if(examId is null)
                throw new InvalidDataException("Exam Id must be given");
            HttpResponseMessage response = await _client.GetAsync($"Submissions/exams/{examId}");
            if (response.IsSuccessStatusCode)
            {
                //read the response
                Stream contentStream = await response.Content.ReadAsStreamAsync();
                await foreach (Submission? sub in JsonSerializer.DeserializeAsyncEnumerable<Submission>(contentStream,_opts))
                {
                    yield return sub;
                }
            }
            yield break;
        }

        public async Task<Exam> AddExam(Exam exam)
        {
            HttpResponseMessage? result = await _client.PostAsJsonAsync<Exam>("Exams/add", exam);
            if (result.IsSuccessStatusCode)
                return exam;
            return null!;
        }
        public async Task<Exam> UpdateExam(Exam exam)
        {
            HttpResponseMessage? result = await _client.PutAsJsonAsync<Exam>("Exams/update", exam);
            if (result.IsSuccessStatusCode)
                return exam;
            return null!;
        }

        public async Task<Exam> GenerateExam(Stream file)
        {
            throw new NotImplementedException();
        }

    }
}
