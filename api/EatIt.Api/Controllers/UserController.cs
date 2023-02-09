using EatIt.Core.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EatIt.Api.Controllers {
    [ApiController]
    [Route("user")]
    public class UserController : Controller {
        private readonly IUserService _userService;

        public UserController(IUserService userService) {
            _userService = userService;
        }

        [HttpGet]
        [Route("profile")]
        [Authorize]
        public async Task<IActionResult> GetUser() {
            var userId = HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) {
                return Forbid();
            }
            var user = _userService.GetUserOverviewAsync(new Guid(userId));
            return user != null ? Ok(user) : NotFound();
        }
    }
}
