namespace QuizApp.API.Services
{
    public class RepositoryResult<T>
    {
        public List<string> Errors { get; private set; } = new();
        public bool IsSuccess { get; private set; }
        public T Result { get; private set; } = default!;

        public static RepositoryResult<T> Success(T item)
        {
            return new RepositoryResult<T>()
            {
                IsSuccess = true,
                Result = item
            };
        }

        public void AddError(string error)
        {
            Errors.Add(error);
        }

        public static RepositoryResult<T> Fail(List<string> errors)
        {
            return new RepositoryResult<T>()
            {
                IsSuccess = false,
                Errors = errors
            };
        }
        public static RepositoryResult<T> Fail(string error)
        {
            return new RepositoryResult<T>()
            {
                IsSuccess = false,
                Errors = new List<string>() { error }
            };
        }
        public static implicit operator T(RepositoryResult<T> item) => item.Result;

    }
}
