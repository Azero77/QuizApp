using QuizApp.Parser.WordFileParser;

namespace QuizApp.API.Handlers
{
    public class ErrorMessager : IMessager
    {
        public event Action<string>? Error;
        public void OnError(string error)
        {
            Error?.Invoke(error);
        }
        public void Message(string message)
        {
            OnError(message);
        }
    }
}
