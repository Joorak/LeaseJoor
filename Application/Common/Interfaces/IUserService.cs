// File: Interfaces/IUserService.cs
using Application.Models;
using Domain.Entities.Identity;

namespace Application.Interfaces
{
    /// <summary>
    /// Defines methods for managing users.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Retrieves the roles assigned to a user.
        /// </summary>
        Task<List<string>> GetUserRoleAsync(User user);

        /// <summary>
        /// Retrieves all active users.
        /// </summary>
        Task<List<UserResponse>> GetUsersAsync();

        /// <summary>
        /// Retrieves all inactive users.
        /// </summary>
        Task<List<UserResponse>> GetUsersInactiveAsync();

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        Task<UserResponse?> GetUserByIdAsync(int userId);

        /// <summary>
        /// Retrieves a user by their email.
        /// </summary>
        UserResponse? GetUserByEmail(string email);

        /// <summary>
        /// Finds a user by their ID.
        /// </summary>
        Task<User?> FindUserByIdAsync(int userId);

        /// <summary>
        /// Finds a user by their email.
        /// </summary>
        Task<User?> FindUserByEmailAsync(string email);

        /// <summary>
        /// Creates a new user.
        /// </summary>
        Task<RequestResponse> CreateUserAsync(CreateAccountRequest user);

        /// <summary>
        /// Assigns a user to a role.
        /// </summary>
        Task<RequestResponse> AssignUserToRoleAsync(AssignUserToRoleRequest user);

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        Task<RequestResponse> UpdateUserAsync(UpdateUserRequest user);

        /// <summary>
        /// Activates a user account.
        /// </summary>
        Task<RequestResponse> ActivateUserAsync(ActivateUserRequest user);

        /// <summary>
        /// Updates a user's email.
        /// </summary>
        Task<RequestResponse> UpdateUserEmailAsync(UpdateUserEmailRequest user);

        /// <summary>
        /// Deletes a user by their ID.
        /// </summary>
        Task<RequestResponse> DeleteUserAsync(int userId);

        /// <summary>
        /// Checks if a user is in a specific role.
        /// </summary>
        Task<bool> IsInRoleAsync(int userId, string role);

        /// <summary>
        /// Authorizes a user based on a policy.
        /// </summary>
        Task<bool> AuthorizeAsync(int userId, string policyName);
    }
}