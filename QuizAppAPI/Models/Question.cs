
namespace Models
{
    public class Question
    {
        public string? QuestionText { get; set; }
        public string? Answer { get; set; }
        public IEnumerable<string> Choices { get; set; } = new List<string>();
    }
}