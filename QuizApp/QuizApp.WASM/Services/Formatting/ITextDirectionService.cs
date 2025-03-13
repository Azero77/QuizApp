namespace QuizApp.BlazorWASM.Services
{
    public interface ITextDirectionService
    {
        IEnumerable<TextSegment> SplitTextIntoSegments(string text);
    }
}
