using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AccountService(
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<RequestResponse> ChangePasswordUserAsync(ChangePasswordRequest changePassword)
        {
            var appUser = await _userManager.FindByIdAsync(changePassword.UserId.ToString()).ConfigureAwait(false);
            if (appUser == null)
            {
                return RequestResponse.Failure("The user does not exist");
            }

            if (!await _userManager.CheckPasswordAsync(appUser, changePassword.OldPassword!).ConfigureAwait(false))
            {
                return RequestResponse.Failure("The credentials are not valid");
            }

            if (!changePassword.NewPassword!.Equals(changePassword.ConfirmNewPassword))
            {
                return RequestResponse.Failure("Passwords do not match");
            }

            await _userManager.ChangePasswordAsync(appUser, changePassword.OldPassword!, changePassword.NewPassword).ConfigureAwait(false);
            return RequestResponse.Success();
        }

        public async Task<bool> CheckPasswordAsync(int userId, string password)
        {
            var appUser = await _userManager.FindByIdAsync(userId.ToString()).ConfigureAwait(false);
            return await _userManager.CheckPasswordAsync(appUser, password).ConfigureAwait(false);
        }

        private JwtTokenResponse GenerateToken(string userName, string role)
        {
            var jwtSettings = new
            {
                Secret = _configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("Jwt:SecretKey is missing"),
                Issuer = _configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer is missing"),
                Audience = _configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience is missing"),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userName),
                new Claim(ClaimTypes.Role, role),
            };

            var expiresIn = DateTime.UtcNow.AddSeconds(double.Parse(_configuration["Jwt:AccessTokenExpiration"] ?? "3600"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Audience = jwtSettings.Audience,
                Issuer = jwtSettings.Issuer,
                Expires = expiresIn,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
            };
            var token = new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);

            return new JwtTokenResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresIn = (int)(expiresIn - DateTime.UtcNow).TotalSeconds,
            };
        }

        public async Task<RequestResponse<JwtTokenResponse>> LoginAsync(LoginRequest login)
        {
            var appUser = await _userManager.FindByNameAsync(login.UserName).ConfigureAwait(false);
            if (appUser == null)
                return RequestResponse<JwtTokenResponse>.Failure("Account not found");

            if (login.RoleForLogin == StringRoleResources.Customer || login.RoleForLogin == StringRoleResources.Supplier)
            {
                if (!await _roleManager.RoleExistsAsync(login.RoleForLogin))
                {
                    return RequestResponse<JwtTokenResponse>.Failure("Invalid role specified");
                }
            }

            var passwordValid = await _userManager.CheckPasswordAsync(appUser, login.PassKey!).ConfigureAwait(false);
            if (!passwordValid)
            {
                return RequestResponse<JwtTokenResponse>.Failure("Email / password incorrect");
            }

            var jwtToken = GenerateToken(login.UserName, login.RoleForLogin);
            return RequestResponse<JwtTokenResponse>.Success(jwtToken);
        }

        public async Task<RequestResponse<JwtTokenResponse>> LoginAdminAsync(string userName)
        {
            var appUser = await _userManager.FindByNameAsync(userName).ConfigureAwait(false);
            if (appUser == null)
                return RequestResponse<JwtTokenResponse>.Failure("Admin account not found");

            var jwtToken = GenerateToken(userName, StringRoleResources.Admin);
            return RequestResponse<JwtTokenResponse>.Success(jwtToken);
        }

        public async Task<RequestResponse<JwtTokenResponse>> RegisterAsync(RegisterRequest register)
        {
            var existUser = await _userManager.FindByNameAsync(register.UserName).ConfigureAwait(false);
            if (existUser != null)
            {
                return RequestResponse<JwtTokenResponse>.Failure("The user with the unique identifier already exists");
            }

            var newUser = new AppUser
            {
                UserName = register.UserName,
                Email = register.UserName,
                FirstName = register.FirstName,
                LastName = register.LastName,
                IsActive = true,
            };

            if (!register.Password!.Equals(register.ConfirmPassword))
            {
                return RequestResponse<JwtTokenResponse>.Failure("Passwords do not match");
            }

            var result = await _userManager.CreateAsync(newUser, register.Password).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                return RequestResponse<JwtTokenResponse>.Failure("User creation failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            var role = await _roleManager.FindByNameAsync(register.RoleForRegister!).ConfigureAwait(false);
            if (role == null)
            {
                return RequestResponse<JwtTokenResponse>.Failure("The role does not exist");
            }

            await _userManager.AddToRoleAsync(newUser, role.Name!).ConfigureAwait(false);
            var jwtToken = GenerateToken(register.UserName, register.RoleForRegister!);
            return RequestResponse<JwtTokenResponse>.Success(jwtToken);
        }

        public async Task<RequestResponse> ResetPasswordUserAsync(ResetPasswordRequest resetPassword)
        {
            var appUser = await _userManager.FindByEmailAsync(resetPassword.Email!).ConfigureAwait(false);
            if (appUser == null)
            {
                return RequestResponse.Failure("The user does not exist");
            }

            if (!resetPassword.NewPassword!.Equals(resetPassword.NewConfirmPassword))
            {
                return RequestResponse.Failure("Passwords do not match");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(appUser).ConfigureAwait(false);
            var result = await _userManager.ResetPasswordAsync(appUser, token, resetPassword.NewPassword).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                return RequestResponse.Failure("Password reset failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            return RequestResponse.Success();
        }

        public async Task<RequestResponse> ValidateTokenAsync(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("Jwt:SecretKey is missing")));
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    IssuerSigningKey = key
                }, out _);
                return RequestResponse.Success();
            }
            catch (Exception ex)
            {
                return RequestResponse.Failure($"Token validation failed: {ex.Message}");
            }
        }

        public async Task<bool> SendPassKeyAsync(string mobileNumber, string passKey)
        {
            // Implement SMS service integration
            await Task.CompletedTask;
            return true;
        }
    }
}