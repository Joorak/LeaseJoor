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
            var appUser = await _userManager.FindByIdAsync(changePassword.UserId.ToString());
            if (appUser == null)
            {
                return RequestResponse.Failure("The user does not exist");
            }

            if (!await _userManager.CheckPasswordAsync(appUser, changePassword.OldPassword!))
            {
                return RequestResponse.Failure("The credentials are not valid");
            }

            if (!changePassword.NewPassword!.Equals(changePassword.ConfirmNewPassword))
            {
                return RequestResponse.Failure("Passwords do not match");
            }

            await _userManager.ChangePasswordAsync(appUser, changePassword.OldPassword!, changePassword.NewPassword);
            return RequestResponse.Success();
        }

        public async Task<bool> CheckPasswordAsync(int userId, string password)
        {
            var appUser = await _userManager.FindByIdAsync(userId.ToString());
            return await _userManager.CheckPasswordAsync(appUser, password);
        }

        private JwtTokenResponse GenerateToken(string userName, string role)
        {
            var jwtSettings = new
            {
                Secret = _configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("Jwt:SecretKey is missing"),
                Issuer = _configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer is missing"),
                Audience = _configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience is missing"),
            };
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

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
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);

            return new JwtTokenResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                //ExpiresIn = new PersianCalendarService(DateTimeOffset.FromUnixTimeSeconds((long)expiresIn.Subtract(DateTime.UtcNow).TotalSeconds).DateTime)
                //ExpiresIn = PersianCalendarService.Now.AddSeconds(int.Parse(_configuration["Jwt:AccessTokenExpiration"]!.ToString() ?? "3600")).ToShortDateString(),
                ExpiresIn = new PersianCalendarService(expiresIn.ToLocalTime()).ToString() 
            };
        }

        public async Task<RequestResponse<JwtTokenResponse>> LoginAsync(LoginRequest loginRequest)
        {
            var appUser = await _userManager.FindByNameAsync(loginRequest.UserName);
            if (appUser == null)
                return RequestResponse<JwtTokenResponse>.Failure("Account not found");
            if (!await _userManager.IsInRoleAsync(appUser, loginRequest.RoleForLogin))
                return RequestResponse<JwtTokenResponse>.Failure("Account not found");

            if (loginRequest.RoleForLogin == StringRoleResources.Customer || loginRequest.RoleForLogin == StringRoleResources.Supplier)
            {
                if (!await _roleManager.RoleExistsAsync(loginRequest.RoleForLogin))
                {
                    return RequestResponse<JwtTokenResponse>.Failure("Invalid role specified");
                }
            }

            var passwordValid = await _userManager.CheckPasswordAsync(appUser, loginRequest.PassKey!);
            if (!passwordValid)
            {
                return RequestResponse<JwtTokenResponse>.Failure("Username / password incorrect");
            }

            var jwtToken = GenerateToken(loginRequest.UserName, loginRequest.RoleForLogin);
            return RequestResponse<JwtTokenResponse>.Success(jwtToken);
        }

        public async Task<RequestResponse<JwtTokenResponse>> LoginAdminAsync(string userName, string passKey)
        {
            var appUser = await _userManager.FindByNameAsync(userName);
            if (appUser == null)
                return RequestResponse<JwtTokenResponse>.Failure("Admin account not found");
            if (!await _userManager.IsInRoleAsync(appUser, StringRoleResources.Admin))
                return RequestResponse<JwtTokenResponse>.Failure("User is not admin");

            var passwordValid = await _userManager.CheckPasswordAsync(appUser, passKey!);
            if (!passwordValid)
            {
                return RequestResponse<JwtTokenResponse>.Failure("Email / password incorrect");
            }
            var jwtToken = GenerateToken(userName, StringRoleResources.Admin);
            return RequestResponse<JwtTokenResponse>.Success(jwtToken);
        }

        public async Task<RequestResponse<JwtTokenResponse>> RegisterAsync(RegisterRequest register)
        {
            var existUser = await _userManager.FindByNameAsync(register.UserName);
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

            var result = await _userManager.CreateAsync(newUser, register.Password);
            if (!result.Succeeded)
            {
                return RequestResponse<JwtTokenResponse>.Failure("User creation failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            var role = await _roleManager.FindByNameAsync(register.RoleForRegister!);
            if (role == null)
            {
                return RequestResponse<JwtTokenResponse>.Failure("The role does not exist");
            }

            await _userManager.AddToRoleAsync(newUser, role.Name!);
            var jwtToken = GenerateToken(register.UserName, register.RoleForRegister!);
            return RequestResponse<JwtTokenResponse>.Success(jwtToken);
        }

        public async Task<RequestResponse> ResetPasswordUserAsync(ResetPasswordRequest resetPassword)
        {
            var appUser = await _userManager.FindByEmailAsync(resetPassword.Email!);
            if (appUser == null)
            {
                return RequestResponse.Failure("The user does not exist");
            }

            if (!resetPassword.NewPassword!.Equals(resetPassword.NewConfirmPassword))
            {
                return RequestResponse.Failure("Passwords do not match");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(appUser);
            var result = await _userManager.ResetPasswordAsync(appUser, token, resetPassword.NewPassword);
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
                await tokenHandler.ValidateTokenAsync(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    IssuerSigningKey = key
                });
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