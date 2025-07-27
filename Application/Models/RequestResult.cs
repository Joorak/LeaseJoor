// File: Models/RequestResult.cs
namespace Application.Models
{
    /// <summary>
    /// Generic result model for API operations.
    /// </summary>
    /// <typeparam name="T">The type of the result data.</typeparam>
    public class RequestResult<T> where T : class
    {
        /// <summary>
        /// Indicates whether the operation was successful.
        /// </summary>
        public bool Successful { get; set; }

        /// <summary>
        /// Error message if the operation failed.
        /// </summary>
        public string? Error { get; set; }

        /// <summary>
        /// The result item (if any).
        /// </summary>
        public T? Item { get; set; }

        /// <summary>
        /// List of result items (if any).
        /// </summary>
        public List<T>? Items { get; set; }
    }
}