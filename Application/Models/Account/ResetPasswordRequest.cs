﻿namespace Application.Models
{
    public class ResetPasswordRequest
    {
        public string? Email { get; set; }
        public string? NewPassword { get; set; }
        public string? NewConfirmPassword { get; set; }
    }
}