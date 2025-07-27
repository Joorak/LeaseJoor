// File: Utils/ErrorsManager.cs
namespace Application.Utils
{
    /// <summary>
    /// Defines common error messages used in the application.
    /// </summary>
    public static class ErrorsManager
    {
        public const string CreateEntity = "خطایی در ایجاد رکورد رخ داد.";
        public const string UpdateEntity = "خطایی در به‌روزرسانی رکورد رخ داد.";
        public const string DeleteEntity = "خطایی در حذف رکورد رخ داد.";
        public const string EntityNotFound = "رکورد موردنظر یافت نشد.";
        public const string InvalidCredentials = "نام کاربری یا رمز عبور اشتباه است.";
        public const string UserAlreadyExists = "کاربر با این مشخصات قبلاً ثبت شده است.";
        public const string InvalidToken = "توکن نامعتبر یا منقضی شده است.";
    }
}