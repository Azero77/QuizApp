using DocumentFormat.OpenXml;

namespace QuizApp.Parser;

public class RawQuestion
{
    public OpenXmlElement QuestionText { get; set; } = null!;
    public OpenXmlElement QuestionChoices { get; set; } = null!;
    public OpenXmlElement QuestionAnswer { get; set; } = null!;
}

