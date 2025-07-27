// File: Models/RoleResponse.cs
using Domain.Entities.Identity;

namespace Application.Models
{
    /// <summary>
    /// Response model for role-related operations.
    /// </summary>
    public class RoleResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string NormalizedName { get; set; } = string.Empty;
    }

}