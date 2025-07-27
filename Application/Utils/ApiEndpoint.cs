// File: Utils/ApiEndpoint.cs
namespace Application.Utils
{
    /// <summary>
    /// Defines API endpoint constants for various modules.
    /// </summary>
    public static class ApiEndpoint
    {
        public static class Account
        {
            public const string Base = "/account";
            public const string Login = Base + "/login";
            public const string Register = Base + "/register";
            public const string ValidateToken = Base + "/validate-token";
            public const string ChangePassword = Base + "/change-password";
            public const string ResetPassword = Base + "/reset-password";
            public const string CheckPassword = Base + "/check-password";
            public const string SendSmsPassKey = Base + "/send-sms-passkey";
            public const string VerifySmsCode = Base + "/verify-sms-code";
            public const string Logout = Base + "/logout";

            public static string GetVersionedEndpoint(string endpoint, string version = "v1")
            {
                return $"/{version}{endpoint}";
            }
        }

        public static class Customer
        {
            public const string Base = "/customer";
            public const string Profile = Base + "/profile";
            public const string Dashboard = Base + "/dashboard";
            public const string Orders = Base + "/orders";
            public const string Contracts = Base + "/contracts";
            public const string Notifications = Base + "/notifications";

            public static string GetVersionedEndpoint(string endpoint, string version = "v1")
            {
                return $"/{version}{endpoint}";
            }
        }

        public static class Supplier
        {
            public const string Base = "/supplier";
            public const string Profile = Base + "/profile";
            public const string Dashboard = Base + "/dashboard";
            public const string Products = Base + "/products";
            public const string Orders = Base + "/orders";
            public const string Contracts = Base + "/contracts";
            public const string Notifications = Base + "/notifications";

            public static string GetVersionedEndpoint(string endpoint, string version = "v1")
            {
                return $"/{version}{endpoint}";
            }
        }

        public static class Admin
        {
            public const string Base = "/admin";
            public const string Dashboard = Base + "/dashboard";
            public const string Users = Base + "/users";
            public const string Reports = Base + "/reports";
            public const string Settings = Base + "/settings";
            public const string Audit = Base + "/audit";

            public static string GetVersionedEndpoint(string endpoint, string version = "v1")
            {
                return $"/{version}{endpoint}";
            }
        }

        public static class Common
        {
            public const string Base = "/common";
            public const string Upload = Base + "/upload";
            public const string Download = Base + "/download";
            public const string Health = Base + "/health";
            public const string Version = Base + "/version";

            public static string GetVersionedEndpoint(string endpoint, string version = "v1")
            {
                return $"/{version}{endpoint}";
            }
        }
    }
}