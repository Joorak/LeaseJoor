using System.Security.Claims;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        private readonly ILogger<AccountController> _logger;
        private readonly IExternalApiService _externalApi;

        public AccountController(
            IAccountService accountService,
            IUserService userService,
            ILogger<AccountController> logger,
            IExternalApiService externalApi)
        {
            _accountService = accountService;
            _userService = userService;
            _logger = logger;
            _externalApi = externalApi;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(RequestResponse<JwtTokenResponse>), 200)]
        [ProducesResponseType(typeof(RequestResponse), 400)]
        [ProducesResponseType(typeof(RequestResponse), 401)]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(RequestResponse.Failure("اطلاعات وارد شده نامعتبر است."));
                }

                _logger.WriteLog($"Login attempt for user: {loginRequest.UserName}");


                var result = await _accountService.LoginAsync(loginRequest);

                if (result.Successful && result.Item != null)
                {
                    _logger.WriteLog($"Login successful for user: {loginRequest.UserName}");
                    return Ok(result);
                }

                _logger.WriteLog($"Login failed for user: {loginRequest.UserName}", result.ErrorMessage);
                return Unauthorized(RequestResponse.Failure("نام کاربری یا رمز عبور اشتباه است."));
            }
            catch (Exception ex)
            {
                _logger.WriteLog(message: $"Error during login for user: {loginRequest.UserName}", logTypeParam: ex.Message, logType: LogType.Error);
                return StatusCode(500, RequestResponse.Failure("خطای داخلی سرور رخ داده است."));
            }
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(RequestResponse<JwtTokenResponse>), 200)]
        [ProducesResponseType(typeof(RequestResponse), 400)]
        [ProducesResponseType(typeof(RequestResponse), 409)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(RequestResponse.Failure("اطلاعات وارد شده نامعتبر است."));
                }

                _logger.WriteLog($"Registration attempt for user: {registerRequest.UserName}");

                var result = await _accountService.RegisterAsync(registerRequest);

                if (result.Successful && result.Item != null)
                {
                    _logger.WriteLog($"Registration successful for user: {registerRequest.UserName}");
                    return Ok(result);
                }

                if (result.ErrorMessage?.Contains("موجود") == true)
                {
                    return Conflict(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.WriteLog(message:$"Error during registration for user: {registerRequest.UserName}", logTypeParam:ex.Message, logType:LogType.Error);
                return StatusCode(500, RequestResponse.Failure("خطای داخلی سرور رخ داده است."));
            }
        }

        [HttpPost("validate-token")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(RequestResponse), 200)]
        [ProducesResponseType(typeof(RequestResponse), 401)]
        public async Task<IActionResult> ValidateToken()
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(RequestResponse.Failure("توکن یافت نشد"));
                }

                var result = await _accountService.ValidateTokenAsync(token);
                if (result.Successful)
                {
                    return Ok(result);
                }

                return Unauthorized(result);
            }
            catch (Exception ex)
            {
                _logger.WriteLog(message: "Error during token validation", logTypeParam: ex.Message, logType: LogType.Error);
                return StatusCode(500, RequestResponse.Failure("خطای داخلی سرور رخ داده است."));
            }
        }

        [HttpPost("change-password")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(RequestResponse), 200)]
        [ProducesResponseType(typeof(RequestResponse), 400)]
        [ProducesResponseType(typeof(RequestResponse), 401)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest changePasswordRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(RequestResponse.Failure("اطلاعات وارد شده نامعتبر است."));
                }

                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(currentUserId))
                {
                    return Unauthorized(RequestResponse.Failure("کاربر احراز هویت نشده است."));
                }

                if (currentUserId != changePasswordRequest.UserId.ToString() && !User.IsInRole("Admin"))
                {
                    return Forbid();
                }

                _logger.WriteLog($"Password change request for user: {changePasswordRequest.UserId}");

                var result = await _accountService.ChangePasswordUserAsync(changePasswordRequest);

                if (result.Successful)
                {
                    _logger.WriteLog($"Password changed successfully for user: {changePasswordRequest.UserId}");
                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.WriteLog(message: $"Error during password change for user: {changePasswordRequest.UserId}", logTypeParam: ex.Message, logType:LogType.Error);
                return StatusCode(500, RequestResponse.Failure("خطای داخلی سرور رخ داده است."));
            }
        }

        [HttpPost("reset-password")]
        [ProducesResponseType(typeof(RequestResponse), 200)]
        [ProducesResponseType(typeof(RequestResponse), 400)]
        [ProducesResponseType(typeof(RequestResponse), 404)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest resetPasswordRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(RequestResponse.Failure("اطلاعات وارد شده نامعتبر است."));
                }

                _logger.WriteLog("Password reset request for email: {Email}", resetPasswordRequest.Email);

                var result = await _accountService.ResetPasswordUserAsync(resetPasswordRequest);

                if (result.Successful)
                {
                    _logger.WriteLog("Password reset successful for email: {Email}", resetPasswordRequest.Email);
                    return Ok(result);
                }

                if (result.ErrorMessage?.Contains("یافت نشد") == true)
                {
                    return NotFound(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.WriteLog(message:$"Error during password reset for email: {resetPasswordRequest.Email}", logTypeParam: ex.Message, logType: LogType.Error);
                return StatusCode(500, RequestResponse.Failure("خطای داخلی سرور رخ داده است."));
            }
        }

        [HttpPost("check-password")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RequestResponse), 400)]
        [ProducesResponseType(typeof(RequestResponse), 401)]
        public async Task<IActionResult> CheckPassword([FromBody] CheckPasswordRequest checkPasswordRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(RequestResponse.Failure("اطلاعات وارد شده نامعتبر است."));
                }

                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(currentUserId))
                {
                    return Unauthorized(RequestResponse.Failure("کاربر احراز هویت نشده است."));
                }

                if (currentUserId != checkPasswordRequest.UserId.ToString() && !User.IsInRole("Admin"))
                {
                    return Forbid();
                }

                _logger.WriteLog($"Password check request for user: {checkPasswordRequest.UserId}");

                var user = await _userService.GetUserByIdAsync(checkPasswordRequest.UserId);
                if (user == null)
                {
                    return NotFound(RequestResponse.Failure("کاربر یافت نشد."));
                }

                var result = await _accountService.CheckPasswordAsync(user.Id, checkPasswordRequest.Password);

                _logger.WriteLog($"Password check completed for user: {checkPasswordRequest.UserId}, Result: {result}");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.WriteLog(message:$"Error during password check for user: {checkPasswordRequest.UserId}", logTypeParam: ex.Message, logType: LogType.Error);
                return StatusCode(500, RequestResponse.Failure("خطای داخلی سرور رخ داده است."));
            }
        }

        [HttpPost("send-sms-passkey")]
        [ProducesResponseType(typeof(RequestResponse), 200)]
        [ProducesResponseType(typeof(RequestResponse), 400)]
        public async Task<IActionResult> SendSmsCode([FromBody] SmsSendRequest smsSendRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(RequestResponse.Failure("اطلاعات وارد شده نامعتبر است."));
                }

                _logger.WriteLog($"SMS code request for mobile: {smsSendRequest.MobileNumber}");

                var passKey = new Random().Next(100000, 999999).ToString();
                var result = await _accountService.SendPassKeyAsync(smsSendRequest.MobileNumber, passKey);

                if (result)
                {
                    return Ok(RequestResponse.Success());
                }

                return BadRequest(RequestResponse.Failure($"خطا در ارسال پیامک به شماره موبایل: {smsSendRequest.MobileNumber}"));
            }
            catch (Exception ex)
            {
                _logger.WriteLog(message: $"Error sending SMS code to: {smsSendRequest.MobileNumber}", logTypeParam: ex.Message, logType:LogType.Error);
                return StatusCode(500, RequestResponse.Failure("خطای داخلی سرور رخ داده است."));
            }
        }
    }
}