// File: Models/DeleteUserRequest.cs
using System.ComponentModel.DataAnnotations;

namespace Application.Models
{
    /// <summary>
    /// Request model for deleting a user.
    /// </summary>
    public class DeleteUserRequest
    {
        /// <summary>
        /// Unique identifier of the user to delete.
        /// </summary>
        [Required(ErrorMessage = "شناسه کاربر الزامی است.")]
        public int Id { get; set; }
    }
}