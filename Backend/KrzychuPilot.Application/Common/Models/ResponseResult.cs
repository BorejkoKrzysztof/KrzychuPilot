namespace KrzychuPilot.Application.Common.Models
{
    public class ResponseResult<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; }
        public string? ErrorMessage { get; }
        public DateTime TimeStamp { get; } = DateTime.UtcNow;

        protected ResponseResult(bool isSuccess, T? data, string? errorMessage) 
        { 
            IsSuccess = isSuccess;
            Data = data;
            ErrorMessage = errorMessage;
        }

        public static ResponseResult<T> Success(T data) => new(true, data, null);
        public static ResponseResult<T> Failure(string errorMessage) => new(false, default, errorMessage);
    }

    public class ResponseResult
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; }
        public DateTime TimeStamp { get; } = DateTime.UtcNow;

        protected ResponseResult(bool isSuccess, string? errorMessage)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public static ResponseResult Success() => new(true, null);
        public static ResponseResult Failure(string errorMessage) => new(false, errorMessage);
    }
}
