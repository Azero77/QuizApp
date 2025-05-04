namespace QuizApp.BlazorWASM.Models;

/// <summary>
/// Determines the state of the application to add functionality to the view
/// </summary>
public enum ExamMode
{
    /// <summary>
    /// The exam is created or edited and can be edited
    /// </summary>
    Alter = 0,
    /// the exam is solved and the answers are evaluated
    /// </summary>
    Corrected = 1,
    /// <summary>
    /// the exam is being solved right now
    /// </summary>
    Read = 2
}
