using QuizApp.Models;
using QuizAppAPI.Contexts;
using QuizAppAPI.Services.ExamQuestions;

namespace QuizAppAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            ConfigureServices(builder);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            IServiceCollection services = builder.Services;
            ConfigureMongoModel(builder, services);
            ConfigureExamQuestionsDb(services);
        }

        private static void ConfigureExamQuestionsDb(IServiceCollection services)
        {
            services.AddScoped<ExamQuestionsContext>();
            services.AddScoped<IExamQuestionsRepository, DbExamQuestionsRepository>();
        }

        private static void ConfigureMongoModel(WebApplicationBuilder builder, IServiceCollection services)
        {
            var modelSection = builder.Configuration.GetSection("ConnectionStrings:QuizExamDatabase");
            services.Configure<MongoDbConnectionModel>(modelSection);
        }

    }

}
