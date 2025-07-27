// File: Models/UserResponse.cs
using Domain.Entities.Identity;

namespace Application.Models
{
    /// <summary>
    /// Response model for user-related operations.
    /// </summary>
    public class UserResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool IsActive { get; set; }
        public string? PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
    }

}