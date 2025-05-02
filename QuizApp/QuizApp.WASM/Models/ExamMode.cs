namespace QuizApp.BlazorWASM.Models;

/// <summary>
/// Determines the state of the application to add functionality to the view
/// </summary>
public enum ExamMode
{
    /// <summary>
    /// The exam is created and can be edited
    /// </summary>
    Create = 0,
    /// <summary>
    /// the exam is edited and can edited also
    /// </summary>
    Edit = 1,
    /// <summary>
    /// the exam is solved and the answers are evaluated
    /// </summary>
    Corrected = 2,
    /// <summary>
    /// the exam is being solved right now
    /// </summary>
    Read = 3
}
