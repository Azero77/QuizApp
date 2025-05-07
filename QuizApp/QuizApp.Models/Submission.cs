using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace QuizApp.Models;

public class Submission
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public string SubmissionPersonName { get; set; } = string.Empty;
    public string ExamId { get; set; } = null!;
    public byte?[] Choices { get; set; } = null!;
    public DateTimeOffset DateSubmitted { get; set; }
    public int Result { get; set; }
}
