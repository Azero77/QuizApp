using DocumentFormat.OpenXml.Presentation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using QuizApp.API.Handlers;
using QuizApp.API.Middlewares;
using QuizApp.API.Services.Submissions;
using QuizApp.Models;
using QuizApp.Parser;
using QuizApp.Parser.WordFileParser;
using QuizApp.Shared;
using QuizApp.Shared.Models;
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
            ConfigureLaunchSchema(builder);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton<IMessager, ErrorMessager>();
            builder.Services.AddWordParser();
            ConfigureRateLimiter(builder);
            ConfigureApplicationDbContext(builder);
            ConfigureAuth(builder);
            ConfigureHttpsRedirection(builder);
            ConfigureCORS(builder);
            ConfigureServices(builder);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            UseSwagger(app);

            //app.UseHttpsRedirection();
            app.UseCors("ClientRequests");
            app.UseAuthorization();
            app.UseMiddleware<RequestTimeoutMiddleware>();

            app.MapControllers();

            app.Run();
        }

        private static void ConfigureRateLimiter(WebApplicationBuilder builder)
        {
            builder.Services.AddRateLimiter(opts =>
            {
                opts.AddFixedWindowLimiter("fixedWindowSlider",
                    opts =>
                    {
                        opts.PermitLimit = 10;
                        opts.QueueLimit = 5;
                        opts.Window = TimeSpan.FromSeconds(2);
                    });
            });
        }

        private static void ConfigureCORS(WebApplicationBuilder builder)
        {
            builder.Services.AddCors(opts =>
            {
                opts.AddPolicy(name: "ClientRequests",
                    p =>
                    {
                        string uri = builder.Configuration?["ClientUrl"] ?? string.Empty;
                        p.WithOrigins(uri)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });
        }

        private static void ConfigureHttpsRedirection(WebApplicationBuilder builder)
        {
            builder.Services.AddHttpsRedirection(opts =>
            {
                opts.HttpsPort = builder.Environment.IsDevelopment() ? 5001 : 443;
            });
        }

        private static void ConfigureAuth(WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
                            opts =>
                            {
                                opts.Authority = "https://localhost:5001";
                                opts.TokenValidationParameters = new()
                                {
                                    ValidateAudience = true,
                                    ValidateIssuer = true,
                                    ValidAudiences = new string[] { "exams", "submissions", "examgenerator" }
                                };
                            });

            builder.Services.AddAuthorization();
        }

        private static void ConfigureApplicationDbContext(WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<ApplicationDbContext>(opts =>
            opts.UseNpgsql(builder.Configuration.GetSection("ConnectionStrings:ApplicationDbConnectionString").Value));
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
        }

        private static void UseSwagger(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
        }

        private static void ConfigureLaunchSchema(WebApplicationBuilder builder)
        {
            if (!builder.Environment.IsDevelopment())
            {
                builder.WebHost.ConfigureKestrel(serverOptions =>
                {
                    serverOptions.ListenAnyIP(80); // HTTP
                    serverOptions.ListenAnyIP(443, listenOptions =>
                    {
                        listenOptions.UseHttps(builder.Configuration["Kestrel:Certificates:Default:Path"] ?? string.Empty,
                                             builder.Configuration["Kestrel:Certificates:Default:Password"]);
                    });
                });
            }
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
            services.AddScoped<ISubmissionRepository, DbSubmissionRepository>();
        }

        private static void ConfigureMongoModel(WebApplicationBuilder builder, IServiceCollection services)
        {
            var modelSection = builder.Configuration.GetSection("ConnectionStrings:QuizExamDatabase");
            services.Configure<MongoDbConnectionModel>(modelSection);
        }

    }

}
