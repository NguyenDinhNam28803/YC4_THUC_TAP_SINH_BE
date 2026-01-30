using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using YC4_THUC_TAP_SINH_BE.Services;

namespace YC4_THUC_TAP_SINH_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService UserService)
        {
            _userService = UserService;
        }
    }
}
