using Microsoft.AspNetCore.Http.HttpResults;
using MongoDB.Driver;
using QuizApp.API.Services;
using QuizApp.Models;
using QuizAppAPI.Contexts;
using System.Runtime.CompilerServices;

namespace QuizAppAPI.Services.ExamQuestions
{
    public class DbExamQuestionsRepository : IExamQuestionsRepository
    {
        private readonly IMongoCollection<Exam> _exams;

        public DbExamQuestionsRepository(ExamQuestionsContext context)
        {
            _exams = context.Exams!;
        }

        public async Task<RepositoryResult<Exam>> AddExam(Exam exam, CancellationToken token = default)
        {
            if (exam.id is not null || (await _exams.FindAsync(e => e.id == exam.id)).Any())
                return RepositoryResult<Exam>.Fail("Item Already Exists");
            await _exams.InsertOneAsync(exam, new InsertOneOptions() { BypassDocumentValidation = false }, token);
            return RepositoryResult<Exam>.Success(exam);
        }

        public async Task<RepositoryResult<Exam>> DeleteExam(string id, CancellationToken token = default)
        {
            try
            {
                var filter = Builders<Exam>.Filter.Eq(e => e.id, id);

                Exam? deletedExam = await _exams.FindOneAndDeleteAsync(filter, cancellationToken: token);
                if (deletedExam is null)
                    return RepositoryResult<Exam>.Fail("Object not found");
                return RepositoryResult<Exam>.Success(deletedExam);
            }
            catch (Exception e)
            {
                return RepositoryResult<Exam>.Fail(e.Message);
            }
        }

        public async Task<RepositoryResult<Exam>> GetExam(string examName, CancellationToken token = default)
        {
            var result = (await _exams.FindAsync(e => e.Name == examName, cancellationToken: token));
            return RepositoryResult<Exam>.Success(result.Single());
        }

        public async Task<RepositoryResult<Exam>> GetExamById(string id,CancellationToken token = default)
        {
            var result = (await _exams.FindAsync(e => e.id == id, cancellationToken: token));
            return RepositoryResult<Exam>.Success(result.Single());
        }

        public async IAsyncEnumerable<Exam> GetExamsAsync([EnumeratorCancellation] CancellationToken token = default)
        {
            var projection = Builders<Exam>.Projection.Include("Id").Include("Name");
            var cursor = await _exams.FindAsync(
                filter: Builders<Exam>.Filter.Empty,
                options: new FindOptions<Exam>() { Projection = projection });
            // Iterate over the cursor asynchronously
            while (await cursor.MoveNextAsync(token))
            {
                foreach (var exam in cursor.Current)
                {
                    // Yield each exam to the caller
                    yield return exam;
                }
            }
        }
       
        public async IAsyncEnumerable<Question> GetQuestionsAsync(string examId, [EnumeratorCancellation] CancellationToken token = default)
        {
            var exam = await GetExamById(examId,token);

            // Iterate over the questions synchronously (since they are in memory)
            foreach (var question in exam.Result.Questions)
            {
                // Yield each question to the caller
                yield return question;
            }
        }

        public async Task<RepositoryResult<Exam>> UpdateExam(Exam exam, CancellationToken token = default)
        {
            try
            {
                var filter = Builders<Exam>.Filter.Eq(e => e.id, exam.id);
                var updateDef = Builders<Exam>.Update
                    .Set(e => e.Name, exam.Name)
                    .Set(e => e.Questions, exam.Questions);
                var options = new FindOneAndUpdateOptions<Exam>
                {
                    ReturnDocument = ReturnDocument.After
                };

                Exam? updateExam = await _exams.FindOneAndUpdateAsync<Exam>(filter, updateDef,options,cancellationToken: token);
                if (updateExam is null)
                    return RepositoryResult<Exam>.Fail("Object not found");
                return RepositoryResult<Exam>.Success(updateExam);
            }
            catch (Exception e)
            {
                return RepositoryResult<Exam>.Fail(e.Message);
            }
        }
    }
}
