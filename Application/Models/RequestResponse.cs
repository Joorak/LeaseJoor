namespace Application.Models
{
    public class RequestResponse
    {
        public bool Successful { get; set; }
        public string? ErrorMessage { get; set; }

        public static RequestResponse Success() => new() { Successful = true };
        public static RequestResponse Failure(string errorMessage) => new() { Successful = false, ErrorMessage = errorMessage };
    }

    public class RequestResponse<T> : RequestResponse
    {
        public T? Item { get; set; }

        public static RequestResponse<T> Success(T item) => new() { Successful = true, Item = item };
        public static new RequestResponse<T> Failure(string errorMessage) => new() { Successful = false, ErrorMessage = errorMessage };
    }
}