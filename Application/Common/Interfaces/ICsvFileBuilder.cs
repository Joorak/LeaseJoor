// File: Interfaces/ICsvFileBuilder.cs
namespace Application.Interfaces
{
    /// <summary>
    /// Defines methods for building CSV files from data.
    /// </summary>
    public interface ICsvFileBuilder
    {
        /// <summary>
        /// Builds a CSV file from a collection of records.
        /// </summary>
        /// <typeparam name="T">The type of records.</typeparam>
        /// <param name="records">The collection of records to convert to CSV.</param>
        /// <returns>A byte array representing the CSV file.</returns>
        byte[] BuildCsvFile<T>(IEnumerable<T> records);
    }
}