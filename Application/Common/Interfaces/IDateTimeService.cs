// File: Interfaces/IDateTimeService.cs
namespace Application.Interfaces
{
    /// <summary>
    /// Defines methods for accessing date and time information.
    /// </summary>
    public interface IDateTimeService
    {
        /// <summary>
        /// Gets the current date and time.
        /// </summary>
        DateTime Now { get; }
    }
}