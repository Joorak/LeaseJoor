namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    [Authorize(Roles = "Admin")]
    public class LeaseJoorController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAccountService _accountService;

        public LeaseJoorController(IConfiguration configuration, IAccountService accountService)
        {
            _configuration = configuration;
            _accountService = accountService;
        }

        [HttpGet("health")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(object), 200)]
        public IActionResult Health()
        {
            return Ok(new { Status = "Healthy", Timestamp = DateTime.UtcNow });
        }

        [HttpGet("connection-string")]
        [ProducesResponseType(typeof(RequestResponse<string>), 200)]
        public IActionResult GetConnectionString()
        {
            try
            {
                var connectionString = _configuration["ConnectionStrings:Default"];
                return Ok(RequestResponse<string>.Success(connectionString));
            }
            catch (Exception ex)
            {
                return BadRequest(RequestResponse.Failure(ex.Message));
            }
        }

        [HttpGet("admin-token")]
        [ProducesResponseType(typeof(RequestResponse<JwtTokenResponse>), 200)]
        [ProducesResponseType(typeof(RequestResponse), 400)]
        public async Task<IActionResult> GetAdminToken([FromQuery] string userName, [FromQuery] string password)
        {
            try
            {
                var adminUserName = _configuration["AdminSeedModel:UserName"];
                var adminPassword = _configuration["AdminSeedModel:Password"];
                if (userName != adminUserName || password != adminPassword)
                {
                    return Unauthorized(RequestResponse.Failure("Invalid credentials"));
                }

                var response = await _accountService.LoginAdminAsync(userName);
                return response.Successful ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                return BadRequest(RequestResponse.Failure(ex.Message));
            }
        }
    }
}