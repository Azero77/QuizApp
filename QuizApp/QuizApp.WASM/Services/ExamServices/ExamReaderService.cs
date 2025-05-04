using Microsoft.AspNetCore.Components;
using QuizApp.BlazorWASM.Models;
using QuizApp.BlazorWASM.Pages;
using QuizApp.Models;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;

namespace QuizApp.BlazorWASM.Services.ExamServices
{
    /// <summary>
    /// Service For Containing info about the exam when the student is solving
    /// </summary>
    public class ExamService
    {
        private readonly ExamsClient _client;
        public Exam Exam { get; set; } = null!;
        public Submission Submission { get; set; } = null!;
        public ExamMode Mode { get; set; } = ExamMode.Read;
        public ExamService(ExamsClient client)
        {
            _client = client;
        }
        public void Initialize(Exam exam,ExamMode mode)
        {
            Exam = exam;
            Mode = mode;
        }
        public void Initialize(Exam exam, ExamMode mode,Submission submission)
        {
            Initialize(exam, mode);
            Submission = submission;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Exam">Exam Populated With student answer to each question</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="HttpRequestException"></exception>
        public Task<HttpResponseMessage> SubmitExam(string submissionName)
        {
            if (Exam is null)
                throw new ArgumentException("Please Assign Exam Before Calling Any method");
            Submission = new Submission()
            {
                SubmissionPersonName = submissionName,
                Choices = Exam.Questions.Select(q => q.SelectedAnswer).ToList(),
                ExamId = Exam.id ?? throw new Exception("Id can't be null here"),
                DateSubmitted = DateTime.Now
            };
            Submission.Result = Exam.CorrectExam(Submission);
            return _client.SubmitExam(Submission);
        }

        public Task<Exam> AddExam()
        {
            return _client.AddExam(Exam);
        }

        public Task<Exam> UpdateExam()
        {
            return _client.UpdateExam(Exam);
        }


        public Task<Exam> DeleteExam(Exam exam)
        {
            throw new NotImplementedException();
        }
    }
}
