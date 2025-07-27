namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    [Authorize(Roles = "Admin")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost("role")]
        [ProducesResponseType(typeof(RequestResponse), 200)]
        [ProducesResponseType(typeof(RequestResponse), 400)]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
        {
            var result = await _roleService.CreateRoleAsync(request);
            return result.Successful ? Ok(result) : BadRequest(result);
        }

        [HttpPut("role")]
        [ProducesResponseType(typeof(RequestResponse), 200)]
        [ProducesResponseType(typeof(RequestResponse), 400)]
        public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleRequest request)
        {
            var result = await _roleService.UpdateRoleAsync(request);
            return result.Successful ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("role/{id}")]
        [ProducesResponseType(typeof(RequestResponse), 200)]
        [ProducesResponseType(typeof(RequestResponse), 400)]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var result = await _roleService.DeleteRoleAsync(id);
            return result.Successful ? Ok(result) : BadRequest(result);
        }

        [HttpGet("role/{id}")]
        [ProducesResponseType(typeof(RequestResponse<RoleResponse>), 200)]
        [ProducesResponseType(typeof(RequestResponse), 400)]
        public async Task<IActionResult> GetRoleById(int id)
        {
            try
            {
                var result = await _roleService.GetRoleByIdAsync(id);
                return Ok(RequestResponse<RoleResponse>.Success(result));
            }
            catch (Exception ex)
            {
                return BadRequest(RequestResponse.Failure(ex.Message));
            }
        }

        [HttpGet("roles")]
        [ProducesResponseType(typeof(RequestResponse<List<RoleResponse>>), 200)]
        [ProducesResponseType(typeof(RequestResponse), 400)]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var result = await _roleService.GetRolesAsync();
                return Ok(RequestResponse<List<RoleResponse>>.Success(result));
            }
            catch (Exception ex)
            {
                return BadRequest(RequestResponse.Failure(ex.Message));
            }
        }
    }
}