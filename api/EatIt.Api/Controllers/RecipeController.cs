using EatIt.Core.Common.Configuration;
using EatIt.Core.Common.DTO.Recipe;
using EatIt.Core.Common.Interfaces;
using EatIt.Core.Models.Atomic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EatIt.Api.Controllers {
    [ApiController]
    [Route("recipe")]
    public class RecipeController : Controller {

        private readonly IRecipeService _recipeService;
        private readonly IUserService _userService;

        public RecipeController(IRecipeService _recipeService, IUserService _userService) {
            this._recipeService = _recipeService;
            this._userService = _userService;
        }

        #region CRUD
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetDetailedRecipeById(Guid id) {
            var recipe = await _recipeService.GetDetailedRecipeByIdAsync(id);
            return recipe != null ? Ok(recipe) : NotFound();
        }

        [HttpPost]
        [Route("")]
        [Authorize]
        public async Task<IActionResult> UpdateRecipe([FromBody]RecipeDetail recipe) {
            var userIdStr = GetUserId();
            if(userIdStr == null) {
                return Forbid();
            }
            var userId = new Guid(userIdStr);
            bool isAdmin = await _userService.IsInRoleAsync(userId, UserRoles.Admin);
            bool isMod = await _userService.IsInRoleAsync(userId, UserRoles.Moderator);
            if (recipe.AuthorId != null && !recipe.AuthorId.Equals(userId) && !(isAdmin || isMod)) {
                return Forbid();
            }
            if(recipe.AuthorId == null)
                recipe.AuthorId = userId;

            var res = await _recipeService.UpdateRecipeAsync(recipe);
            return res.Result.Succeeded ? Ok(res.Recipe) : UnprocessableEntity(res.Result.Errors);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteRecipe(Guid id) {
            var userIdStr = GetUserId();
            if (userIdStr == null) {
                return Forbid();
            }
            var userId = new Guid(userIdStr);
            bool isAdmin = await _userService.IsInRoleAsync(userId, UserRoles.Admin);
            bool isMod = await _userService.IsInRoleAsync(userId, UserRoles.Moderator);
            // Admin and mods can edit
            if (isAdmin || isMod) {
                var result = await _recipeService.DeleteRecipeAsync(id);
                return result.Result.Succeeded ? Ok(result.Recipe) : UnprocessableEntity(result.Result.Errors);
            }
            // is this the users own recipe? 
            var recipe = await _recipeService.GetDetailedRecipeByIdAsync(id);
            if(recipe?.AuthorId.Equals(userId) ?? false) {
                var result = await _recipeService.DeleteRecipeAsync(id);
                return result.Result.Succeeded ? Ok(result.Recipe) : UnprocessableEntity(result.Result.Errors);
            }
            return Forbid();
        }

        #endregion
        #region Searching

        [HttpGet]
        [Route("search/starts")]
        public async Task<IActionResult> GetRecipesByNameStartsWith(string title) {
            var res = await _recipeService.GetRecipesByNameStartsWithAsync(title);
            return Ok(res);
        }

        [HttpGet]
        [Route("search/contains")]
        public async Task<IActionResult> GetRecipesByNameContains(string title) {
            var res = await _recipeService.GetRecipesByNameContainsAsync(title);
            return Ok(res);
        }

        [HttpPost]
        [Route("search/category")]
        public async Task<IActionResult> GetRecipesByCategories([FromBody]List<string> categories) {
            var res = await _recipeService.GetRecipesByCategoryAsync(categories);
            return Ok(res);
        }

        [HttpPost]
        [Route("search/ingredients")]
        public async Task<IActionResult> GetRecipesByIngredients([FromBody]List<Ingredient> ingredients) {
            var res = await _recipeService.GetRecipesWithIngredientsAsync(ingredients);
            return Ok(res);
        }

        #endregion

        [HttpGet]
        [Route("categories/startswith")]
        public async Task<IActionResult> GetCategoriesStartsWith(string name) {
            var res = await _recipeService.GetCategoriesStartsWith(name);
            return Ok(res);
        }

        [HttpGet]
        [Route("categories/{id:Guid}")]
        public async Task<IActionResult> GetCategoriesContains(Guid id) {
            var res = await _recipeService.GetCategoryByIdAsync(id);
            return Ok(res);
        }

        [HttpGet]
        [Route("categories")]
        public async Task<IActionResult> GetAllCategories() {
            var res = await _recipeService.GetAllCategories();
            return Ok(res);
        }

        private string? GetUserId() {
            return HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
