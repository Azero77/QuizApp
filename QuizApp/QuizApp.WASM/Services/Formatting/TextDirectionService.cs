using QuizApp.BlazorWASM.Services;
using System.Text.RegularExpressions;

public class TextDirectionService : ITextDirectionService
{
    public IEnumerable<TextSegment> SplitTextIntoSegments(string text)
    {
        var segments = new List<TextSegment>();
        var mathRegex = new Regex(@"\\\(.*?\\\)|\$\$.*?\$\$"); // Regex to detect MathJax equations

        int lastIndex = 0;
        foreach (Match match in mathRegex.Matches(text))
        {
            // Add non-math text
            if (match.Index > lastIndex)
            {
                segments.Add(new TextSegment
                {
                    Text = text.Substring(lastIndex, match.Index - lastIndex),
                    Direction = IsArabic(text.Substring(lastIndex, match.Index - lastIndex)) ? "rtl" : "ltr",
                    IsMath = false
                });
            }

            // Add math text
            segments.Add(new TextSegment
            {
                Text = match.Value,
                Direction = "ltr", // MathJax equations are always LTR
                IsMath = true
            });

            lastIndex = match.Index + match.Length;
        }

        // Add remaining non-math text
        if (lastIndex < text.Length)
        {
            segments.Add(new TextSegment
            {
                Text = text.Substring(lastIndex),
                Direction = IsArabic(text.Substring(lastIndex)) ? "rtl" : "ltr",
                IsMath = false
            });
        }

        return segments;
    }

    private bool IsArabic(string text)
    {
        return text.Any(c => c >= 0x0600 && c <= 0x06FF);
    }
}

public class TextSegment
{
    public string Text { get; set; } = string.Empty;
    public string Direction { get; set; } = string.Empty;
    public bool IsMath { get; set; }
}