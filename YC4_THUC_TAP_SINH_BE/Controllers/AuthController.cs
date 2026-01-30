using Microsoft.AspNetCore.Http;
using YC4_THUC_TAP_SINH_BE.Dto;
using Microsoft.AspNetCore.Mvc;
using YC4_THUC_TAP_SINH_BE.Services;

namespace YC4_THUC_TAP_SINH_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthInterface _authService;
        public AuthController(IAuthInterface authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                var response = await _authService.Login(loginRequest);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            try
            {
                var response = await _authService.Register(registerRequest);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
