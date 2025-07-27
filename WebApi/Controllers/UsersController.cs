namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("user")]
        [ProducesResponseType(typeof(RequestResponse), 200)]
        [ProducesResponseType(typeof(RequestResponse), 400)]
        public async Task<IActionResult> CreateUser([FromBody] CreateAccountRequest request)
        {
            var result = await _userService.CreateUserAsync(request);
            return result.Successful ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("userActivate")]
        [ProducesResponseType(typeof(RequestResponse), 200)]
        [ProducesResponseType(typeof(RequestResponse), 400)]
        public async Task<IActionResult> ActivateUser([FromBody] ActivateUserRequest request)
        {
            var result = await _userService.ActivateUserAsync(request);
            return result.Successful ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Admin,User,Default")]
        [HttpPut("user")]
        [ProducesResponseType(typeof(RequestResponse), 200)]
        [ProducesResponseType(typeof(RequestResponse), 400)]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
        {
            var result = await _userService.UpdateUserAsync(request);
            return result.Successful ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("userEmail")]
        [ProducesResponseType(typeof(RequestResponse), 200)]
        [ProducesResponseType(typeof(RequestResponse), 400)]
        public async Task<IActionResult> UpdateUserEmail([FromBody] UpdateUserEmailRequest request)
        {
            var result = await _userService.UpdateUserEmailAsync(request);
            return result.Successful ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("user/{id}")]
        [ProducesResponseType(typeof(RequestResponse), 200)]
        [ProducesResponseType(typeof(RequestResponse), 400)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            return result.Successful ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Admin,User,Default")]
        [HttpGet("user/{id}")]
        [ProducesResponseType(typeof(RequestResponse<UserResponse>), 200)]
        [ProducesResponseType(typeof(RequestResponse), 400)]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var result = await _userService.GetUserByIdAsync(id);
                return Ok(RequestResponse<UserResponse>.Success(result));
            }
            catch (Exception ex)
            {
                return BadRequest(RequestResponse.Failure(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("users")]
        [ProducesResponseType(typeof(RequestResponse<List<UserResponse>>), 200)]
        [ProducesResponseType(typeof(RequestResponse), 400)]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var result = await _userService.GetUsersAsync();
                return Ok(RequestResponse<List<UserResponse>>.Success(result));
            }
            catch (Exception ex)
            {
                return BadRequest(RequestResponse.Failure(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("usersInactive")]
        [ProducesResponseType(typeof(RequestResponse<List<UserResponse>>), 200)]
        [ProducesResponseType(typeof(RequestResponse), 400)]
        public async Task<IActionResult> GetUsersInactive()
        {
            try
            {
                var result = await _userService.GetUsersInactiveAsync();
                return Ok(RequestResponse<List<UserResponse>>.Success(result));
            }
            catch (Exception ex)
            {
                return BadRequest(RequestResponse.Failure(ex.Message));
            }
        }
    }
}