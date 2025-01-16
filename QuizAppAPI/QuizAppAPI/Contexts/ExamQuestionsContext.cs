using Microsoft.Extensions.Options;
using Models;
using MongoDB.Driver;

namespace QuizAppAPI.Contexts
{

    /// <summary>
    /// context to connect to mongo db database and return the collection
    /// </summary>
    public class ExamQuestionsContext
    {
        public IMongoCollection<Exam>? Exams { get; set; }
        public ExamQuestionsContext(IOptions<MongoDbConnectionModel> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            var database = client.GetDatabase(options.Value.Database);
            Exams = database.GetCollection<Exam>(options.Value.Collection);
        }
    }
}
