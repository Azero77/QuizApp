using MongoDB.Bson.Serialization.Attributes;
namespace QuizApp.Models;

public class Submission
{
    public string? Id { get; set; }
    public string SubmissionPersonName { get; set; } = string.Empty;
    public string ExamId { get; set; } = null!;
    public byte?[] Choices { get; set; } = null!;

    public DateTime DateSubmitted { get; set; }
    public int Result { get; set; }
}
