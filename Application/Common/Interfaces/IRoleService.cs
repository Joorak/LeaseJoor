// File: Interfaces/IRoleService.cs
using Application.Models;
using Domain.Entities.Identity;

namespace Application.Interfaces
{
    /// <summary>
    /// Defines methods for managing roles.
    /// </summary>
    public interface IRoleService
    {
        /// <summary>
        /// Retrieves the roles assigned to a user.
        /// </summary>
        Task<List<string>> CheckUserRolesAsync(User user);

        /// <summary>
        /// Gets the default role.
        /// </summary>
        RoleResponse? GetDefaultRole();

        /// <summary>
        /// Gets the user role.
        /// </summary>
        RoleResponse? GetUserRole();

        /// <summary>
        /// Gets the admin role.
        /// </summary>
        RoleResponse? GetAdminRole();

        /// <summary>
        /// Assigns a role to a user.
        /// </summary>
        Task<RequestResponse> SetUserRoleAsync(User user, string role);

        /// <summary>
        /// Finds a role by its ID.
        /// </summary>
        Task<Role?> FindRoleByIdAsync(int roleId);

        /// <summary>
        /// Finds a role by its name.
        /// </summary>
        Task<Role?> FindRoleByNameAsync(string name);

        /// <summary>
        /// Creates a new role.
        /// </summary>
        Task<RequestResponse> CreateRoleAsync(CreateRoleRequest role);

        /// <summary>
        /// Updates an existing role.
        /// </summary>
        Task<RequestResponse> UpdateRoleAsync(UpdateRoleRequest role);

        /// <summary>
        /// Deletes a role by its ID.
        /// </summary>
        Task<RequestResponse> DeleteRoleAsync(int roleId);

        /// <summary>
        /// Retrieves all roles.
        /// </summary>
        Task<List<RoleResponse>> GetRolesAsync();

        /// <summary>
        /// Retrieves roles for admin users.
        /// </summary>
        List<RoleResponse> GetRolesForAdmin();

        /// <summary>
        /// Retrieves a role by its ID.
        /// </summary>
        Task<RoleResponse?> GetRoleByIdAsync(int id);

        /// <summary>
        /// Retrieves a role by its normalized name.
        /// </summary>
        RoleResponse? GetRoleByNormalizedName(string normalizedName);
    }
}