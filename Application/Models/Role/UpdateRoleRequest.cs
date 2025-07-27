// File: Models/UpdateRoleRequest.cs
using System.ComponentModel.DataAnnotations;

namespace Application.Models
{
    /// <summary>
    /// Request model for updating an existing role.
    /// </summary>
    public class UpdateRoleRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}