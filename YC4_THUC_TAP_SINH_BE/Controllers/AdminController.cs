using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using YC4_THUC_TAP_SINH_BE.Services;
using YC4_THUC_TAP_SINH_BE.Models;

namespace YC4_THUC_TAP_SINH_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserInterface _userService;
        private readonly IRoleInterface _roleInterface;
        private readonly IFunctionInterface _functionInterface;
        public AdminController(IUserInterface userService, IRoleInterface roleInterface, IFunctionInterface functionInterface)
        {
            _userService = userService;
            _roleInterface = roleInterface;
            _functionInterface = functionInterface;
        }
        // VÍ dụ về phần quyền cho admin
        [HttpGet("admin-only")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminOnly()
        {
            var claims = User.Claims.Select(c => new
            {
                type = c.Type,
                value = c.Value
            }).ToList();

            return Ok(claims);
        }

        [HttpGet]
        [Route("user")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetAllUser()
        {
            try
            {
                var listUser = await _userService.GetAllAsync();
                return Ok(new
                {
                    Success = true,
                    Message = "Lấy danh sách người dùng thành công",
                    Data = listUser
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message,
                });
            }
        }

        [HttpPost]
        [Route("user")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            try
            {
                var response = await _userService.CreateAsync(user);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message,
                });
            }
        }

        [HttpGet]
        [Route("role")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetAllRole()
        {
            try
            {
                var listRole = await _roleInterface.GetAllAsync();
                return Ok(new
                {
                    Success = true,
                    Message = "Lấy danh sách vai trò thành công",
                    Data = listRole
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message,
                });
            }
        }

        [HttpGet]
        [Route("function")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetAllFunction()
        {
            try
            {
                var listRole = await _functionInterface.GetAllAsync();
                return Ok(new
                {
                    Success = true,
                    Message = "Lấy danh sách chức năng thành công",
                    Data = listRole
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message,
                });
            }
        }
    }
}

