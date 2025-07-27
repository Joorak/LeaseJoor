// File: Models/UpdateUserEmailRequest.cs
using System.ComponentModel.DataAnnotations;

namespace Application.Models
{
    /// <summary>
    /// Request model for updating a user's email.
    /// </summary>
    public class UpdateUserEmailRequest
    {
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}