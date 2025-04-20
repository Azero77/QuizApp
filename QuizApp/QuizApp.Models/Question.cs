using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace QuizApp.Models
{
    public class Question
    {
        public IList<string> QuestionChoices { get; set; } = null!;
        public string QuestionText { get; set; } = string.Empty;
        public byte? QuestionAnswer { get; set; }
    }

    //every question segment contains sentences that have some properties


    public class QuestionSentence
    {
        public string Text { get; set; } = string.Empty;
        public QuestionSentenceType QuestionSentenceType { get; set; } = QuestionSentenceType.SimpleText;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? AltText { get; set; } = null;

    }

    public enum QuestionSentenceType
    {
        SimpleText,
        ParagraphEquation,
        InlineEquation,
        ImageUrl,
        CodeBlock,
        Table
    }
}