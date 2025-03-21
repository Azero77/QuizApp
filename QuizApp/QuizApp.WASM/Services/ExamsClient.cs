﻿using QuizApp.BlazorWASM.Pages;
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
            if (subId is null)
                throw new InvalidDataException("Submission Id must be given");
            return _client.GetFromJsonAsync<Submission>($"Submissions/id/{subId}");
        }

        public IAsyncEnumerable<Submission?> GetSubmissionsByExamId(string examId)
        {
            if(examId is null)
                throw new InvalidDataException("Exam Id must be given");
            return _client.GetFromJsonAsAsyncEnumerable<Submission?>($"Submissions/exams/{examId}");
        }
    }
}
