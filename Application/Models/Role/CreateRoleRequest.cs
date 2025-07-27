// File: Models/CreateRoleRequest.cs
using System.ComponentModel.DataAnnotations;

namespace Application.Models
{
    /// <summary>
    /// Request model for creating a new role.
    /// </summary>
    public class CreateRoleRequest
    {
        public string Name { get; set; } = string.Empty;
    }
}