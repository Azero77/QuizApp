using Microsoft.Extensions.DependencyInjection;
using QuizApp.Parser.WordFileParser;
using SharpCompress.Writers;
using WordDocumentTableParserProject.Formatter;
using WordDocumentTableParserProject.QuestionParsers;
using WordDocumentTableParserProject.Selector;
using WordDocumentTableParserProject.WordFileParser;
using IQuestionFormatter = WordDocumentTableParserProject.Formatter.IQuestionFormatter;

namespace QuizApp.Parser;

public static class ServiceExtension
{
    public static IServiceCollection AddWordParser(this IServiceCollection services
        , Func<IMessager> messagerDelegate
        ,Func<IWriter> writerDelegate = null!)
    {
        if(writerDelegate is not null)
            services.AddSingleton<IWriter>(sp => writerDelegate());
        services.AddSingleton<IMessager>(sp => messagerDelegate());
        return AddWordParser(services);
    }

    public static IServiceCollection AddWordParser(this IServiceCollection services)
    {
        services.AddSingleton<IQuestionParser, AnasQuestionParser>();
        services.AddSingleton<IQuestionFormatter, QuestionFormatter>();
        services.AddSingleton<IQuestionSelector, QuestionSelector>();
        services.AddSingleton<IFileParser, WordDocumentParser>();
        return services;
    }
}
