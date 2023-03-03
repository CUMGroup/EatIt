using EatIt.Core.Common.DTO.Shopping;
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

        [HttpGet]
        [Route("shopping")]
        [Authorize]
        public async Task<IActionResult> GetUserShoppingList() {
            var userId = GetUserId();
            if (userId == null) {
                return Forbid();
            }
            var shop = await _userService.GetShoppingListAsync(new Guid(userId));
            return shop != null ? Ok(shop) : NotFound();
        }

        [HttpDelete]
        [Route("shopping")]
        [Authorize]
        public async Task<IActionResult> ClearUserShoppingList() {
            var userId = GetUserId();
            if (userId == null) {
                return Forbid();
            }
            var res = await _userService.ClearShoppingListAsync(new Guid(userId));
            return res.Result.Succeeded ? Ok(res.Ingredients) : UnprocessableEntity(res.Result.Errors); 
        }

        [HttpPost]
        [Route("shopping/ingredient")]
        [Authorize]
        public async Task<IActionResult> AddIngredientToShoppingList(IEnumerable<ShoppingIngredientModel> ingredients) {
            var userId = GetUserId();
            if (userId == null) {
                return Forbid();
            }
            var res = await _userService.AddIngredientsToShoppingListAsync(new Guid(userId), ingredients);
            return res.Result.Succeeded ? Ok(res.Ingredients) : UnprocessableEntity(res.Result.Errors);
        }

        [HttpPost]
        [Route("shopping/recipe")]
        [Authorize]
        public async Task<IActionResult> AddRecipeToShoppingList(RecipeToShoppingListModel recipe) {
            var userId = GetUserId();
            if (userId == null) {
                return Forbid();
            }
            var res = await _userService.AddRecipeIngredientsToShoppingListAsync(new Guid(userId), recipe);
            return res.Result.Succeeded ? Ok(res.Ingredients) : UnprocessableEntity(res.Result.Errors);
        }

        [HttpDelete]
        [Route("shopping/ingredient")]
        [Authorize]
        public async Task<IActionResult> RemoveIngredientFromShoppingList(ShoppingIngredientModel ingredient) {
            var userId = GetUserId();
            if (userId == null) {
                return Forbid();
            }
            var res = await _userService.RemoveFromShoppingListAsync(new Guid(userId), ingredient);
            return res.Result.Succeeded ? Ok(res.Ingredients) : UnprocessableEntity(res.Result.Errors);
        }
    }
}
