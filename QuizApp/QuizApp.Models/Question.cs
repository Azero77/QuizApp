using System.Text.Json.Serialization;

namespace QuizApp.Models
{
    public class Question
    {
        public List<List<QuestionSentence>> QuestionChoices { get; set; } = null!; //every choice is a list of Question segment
        public List<QuestionSentence> QuestionText { get; set; } = null!;
        
        public byte? Answer { get; set; }
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