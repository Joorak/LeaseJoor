// File: Models/ActivateUserRequest.cs
using System.ComponentModel.DataAnnotations;

namespace Application.Models
{
    /// <summary>
    /// Request model for activating a user account.
    /// </summary>
    public class ActivateUserRequest
    {
        public int UserId { get; set; }
        public bool IsActive { get; set; }
    }
}