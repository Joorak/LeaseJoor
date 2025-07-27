using Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    [Authorize(Roles = "Admin")]
    public class LeaseJoorController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAccountService _accountService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ReportingDb _dbContext;
        private readonly ILogger<AccountController> _logger;

        public LeaseJoorController(IWebHostEnvironment webHostEnvironment, ReportingDb reportContext, IConfiguration configuration, ILogger<AccountController> logger, IAccountService accountService)
        {
            _logger = logger;
            _configuration = configuration;
            _accountService = accountService;
            _webHostEnvironment = webHostEnvironment;
            _dbContext = reportContext ?? throw new ArgumentNullException(nameof(reportContext), "ReportingDb context cannot be null");
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

        [HttpGet("admin-login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(RequestResponse<JwtTokenResponse>), 200)]
        [ProducesResponseType(typeof(RequestResponse), 400)]
        public async Task<IActionResult> GetAdminToken([FromQuery] string userName, [FromQuery] string password)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(RequestResponse.Failure("اطلاعات وارد شده نامعتبر است."));
                }

                _logger.LogInformation("Login attempt for user: {UserName}", userName);

                var result = await _accountService.LoginAdminAsync(userName, password);

                if (result.Successful && result.Item != null)
                {
                    _logger.LogInformation("Login successful for user: {UserName}", userName);
                    return Ok(result);
                }

                _logger.LogWarning("Login failed for user: {UserName}", userName);
                return Unauthorized(RequestResponse.Failure("نام کاربری یا رمز عبور اشتباه است."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for user: {UserName}", userName);
                return StatusCode(500, RequestResponse.Failure("خطای داخلی سرور رخ داده است."));
            }
        }

        [HttpGet("getExcelData")]
        [AllowAnonymous]
        public async Task<IActionResult> GetExcelData()
        {

            string filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Files", "Countries_Leasing_Stats.xlsx");
            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    string sql = $"SELECT * FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0', 'Excel 12.0;Database={filePath};HDR=YES;' ,'SELECT * FROM [Sheet1$]')";
                    var result = await _dbContext.CountriesTurnoverReport.FromSqlRaw(sql).AsNoTracking().ToListAsync();
                    return Ok(new RequestResult<CountriesTurnoverStat> { Items = result, Successful = true, Error = null, Item = null });
                }
                catch (Exception ex)
                {
                    return BadRequest(new RequestResult<UserResponse> { Item = null, Successful = false, Error = ex.Message, Items = null });
                }

            }
            return BadRequest(new RequestResult<UserResponse> { Item = null, Successful = false, Error = "Data Not Found...", Items = null });
        }
    }
}