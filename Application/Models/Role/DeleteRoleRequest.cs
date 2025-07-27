// File: Models/DeleteRoleRequest.cs
using System.ComponentModel.DataAnnotations;

namespace Application.Models
{
    /// <summary>
    /// Request model for deleting a role.
    /// </summary>
    public class DeleteRoleRequest
    {
        /// <summary>
        /// Unique identifier of the role to delete.
        /// </summary>
        [Required(ErrorMessage = "شناسه نقش الزامی است.")]
        public int Id { get; set; }
    }
}