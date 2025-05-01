using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordDocumentTableParserProject.Ai;
using WordDocumentTableParserProject.Formatter;
using WordDocumentTableParserProject.QuestionParsers;
using WordDocumentTableParserProject.Selector;
using WordDocumentTableParserProject.WordFileParser;
using WordDocumentTableParserProject.Writer;

namespace WordDocumentTableParserProject
{
    public static class ServicesExtension
    {
        public static void AddWordParserService(this IServiceCollection services,
                                                string word_document_path,
                                                string json_file_path)
        {
            services.AddSingleton<IWriter, JsonFileWriter>(sp => new JsonFileWriter(json_file_path));
            services.AddSingleton<IQuestionParser, AnasQuestionParser>();
            services.AddSingleton<Formatter.IQuestionFormatter, QuestionFormatter>();
            services.AddSingleton<IQuestionSelector, QuestionSelector>();
            services.AddSingleton<WordDocumentParser>(sp => new WordDocumentParser(
                word_document_path,
                sp.GetRequiredService<IWriter>(),
                sp.GetRequiredService<IQuestionSelector>()
                ));
            services.AddSingleton<IConfiguration>(sp => Configuration);
            services.AddSingleton<HttpClient>();
            services.AddSingleton<IAIModel, QuizAIModel>();
        }

        public static IConfiguration Configuration
        {
            get
            {
                if (_configuration is null)
                {
                    var builder = new ConfigurationBuilder()
                      .SetBasePath(Directory.GetCurrentDirectory())
                      .AddJsonFile("config.json", optional: false, reloadOnChange: true);

                    _configuration =  builder.Build();
                }
                 return _configuration;
            }
        }

        private static IConfiguration? _configuration = null;
    }
}
