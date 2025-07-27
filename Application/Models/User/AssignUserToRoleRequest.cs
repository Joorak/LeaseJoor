// File: Models/AssignUserToRoleRequest.cs
using System.ComponentModel.DataAnnotations;

namespace Application.Models
{
    /// <summary>
    /// Request model for assigning a user to a role.
    /// </summary>
    public class AssignUserToRoleRequest
    {
        /// <summary>
        /// Unique identifier of the user.
        /// </summary>
        [Required(ErrorMessage = "شناسه کاربر الزامی است.")]
        public int UserId { get; set; }

        /// <summary>
        /// Unique identifier of the role.
        /// </summary>
        [Required(ErrorMessage = "شناسه نقش الزامی است.")]
        public int RoleId { get; set; }
    }
}