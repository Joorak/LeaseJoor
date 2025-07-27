// File: Interfaces/ICurrentUserService.cs
namespace Application.Interfaces
{
    /// <summary>
    /// Defines methods for accessing the current authenticated user.
    /// </summary>
    public interface ICurrentUserService
    {
        /// <summary>
        /// Gets the unique identifier of the current user.
        /// </summary>
        int UserId { get; }
    }
}