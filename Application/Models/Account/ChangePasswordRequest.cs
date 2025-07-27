namespace Application.Models
{
    public class ChangePasswordRequest
    {
        public int UserId { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? ConfirmNewPassword { get; set; }
    }
}