using EatIt.Core.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EatIt.Api.Controllers {
    [ApiController]
    [Route("user")]
    public class UserController : Controller {
        private readonly IUserService _userService;
        private readonly IRecipeService _recipeService;

        public UserController(IUserService userService, IRecipeService _recipeService) {
            _userService = userService;
            this._recipeService = _recipeService;
        }

        [HttpGet]
        [Route("profile")]
        [Authorize]
        public async Task<IActionResult> GetUser() {
            var userId = GetUserId();
            if (userId == null) {
                return Forbid();
            }
            var user = await _userService.GetUserOverviewAsync(new Guid(userId));
            return user != null ? Ok(user) : NotFound();
        }


        [HttpGet]
        [Route("starred")]
        [Authorize]
        public async Task<IActionResult> GetUserStarred() {
            var userId = GetUserId();
            if(userId == null) {
                return Forbid();
            }
            var recipes = await _recipeService.GetUserStarredAsync(new Guid(userId));

            return recipes != null ? Ok(recipes) : NotFound();
        }

        [HttpGet]
        [Route("created")]
        [Authorize]
        public async Task<IActionResult> GetUserCreated() {
            var userId = GetUserId();
            if (userId == null) {
                return Forbid();
            }
            var recipes = await _recipeService.GetUserCreationsAsync(new Guid(userId));

            return recipes != null ? Ok(recipes) : NotFound();
        }

        private string? GetUserId() {
            return HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
