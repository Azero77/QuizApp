using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace QuizApp.Models;

public class Submission
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public required string UserId { get; set; }
    public ApplicationUser? User { get; set; }
    public required string ExamId { get; set; }
    public byte?[] Choices { get; set; } = null!;
    public required DateTimeOffset DateSubmitted { get; set; }
    public int Result { get; set; }
}
