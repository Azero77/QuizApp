namespace WordDocumentTableParserProject.Ai
{
    public partial class QuizAIModel
    {
        public class AIModelResult
        {
            public bool Succeded => Errors is null;
            public IEnumerable<string>? Errors { get; set; } = null;
            public string result = string.Empty;
            public static AIModelResult Success(string r)
            {
                return new AIModelResult() 
                {
                    result = r
                };
            }
            public static AIModelResult Fail(string errorMessage)
            {
                return new AIModelResult()
                {
                    Errors = new string[] { errorMessage }
                };
            }

            public static AIModelResult Fail(string[] errorMessages)
            {
                return new AIModelResult()
                {
                    Errors = errorMessages
                };
            }
        }
    }
}
