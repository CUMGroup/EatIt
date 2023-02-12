using EatIt.Core.Common.Configuration;
using EatIt.Core.Common.DTO.Ingredient;
using EatIt.Core.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EatIt.Api.Controllers {
    [ApiController]
    [Route("ingredient")]
    public class IngredientController : Controller {

        private readonly IIngredientService _ingredientService;
        private readonly IUserService _userService;
        public IngredientController(IIngredientService ingredientService, IUserService userService) {
            _ingredientService = ingredientService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllIngredients() {
            return Ok(await _ingredientService.GetAllIngredientsAsync());
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetIngredientById(Guid id) {
            var res = await _ingredientService.GetIngredientByIdAsync(id);
            return res != null ? Ok(res) : NotFound();
        }

        [HttpGet]
        [Route("search/starts")]
        public async Task<IActionResult> GetIngredientsByNameStartsWith(string name) {
            return Ok(await _ingredientService.GetIngredientsStartsWithAsync(name));
        }

        [HttpGet]
        [Route("search/contains")]
        public async Task<IActionResult> GetIngredientsByContains(string name) {
            return Ok(await _ingredientService.GetIngredientsContainingAsync(name));
        }

        [HttpPost]
        [Route("search/category")]
        public async Task<IActionResult> GetIngredientByCategories([FromBody]List<string> categories) {
            return Ok(await _ingredientService.GetIngredientsWithCategoryAsync(categories));
        }

        [HttpGet]
        [Route("categories")]
        public async Task<IActionResult> GetAllCategories() {
            return Ok(await _ingredientService.GetAllIngredientCategoriesAsync());
        }

        [HttpGet]
        [Route("categories/starts")]
        public async Task<IActionResult> GetAllCategoriesStartsWith(string name) {
            return Ok(await _ingredientService.GetAllIngredientCategoriesStartsWithAsync(name));
        }


        [HttpPost]
        [Route("")]
        [Authorize]
        public async Task<IActionResult> UpdateIngredient(IngredientModel ingredient) {
            var userIdStr = GetUserId();
            if (userIdStr == null)
                return Forbid();
            var userId = new Guid(userIdStr);
            bool isAdmin = await _userService.IsInRoleAsync(userId, UserRoles.Admin);
            bool isMod = await _userService.IsInRoleAsync(userId, UserRoles.Moderator);
            if(ingredient.Id != null && !(isAdmin || isMod))
                return Forbid();

            var res = await _ingredientService.UpdateIngredientAsync(ingredient);
            return res.Result.Succeeded ? Ok(res.Ingredient) : UnprocessableEntity(res.Result.Errors);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Moderator)]
        public async Task<IActionResult> DeleteIngredient(Guid id) {
            var userIdStr = GetUserId();
            if (userIdStr == null)
                return Forbid();
            var userId = new Guid(userIdStr);
            bool isAdmin = await _userService.IsInRoleAsync(userId, UserRoles.Admin);
            bool isMod = await _userService.IsInRoleAsync(userId, UserRoles.Moderator);
            if(!(isAdmin || isMod)) 
                return Forbid();

            var res = await _ingredientService.DeleteIngredientAsync(id);
            return res.Result.Succeeded ? Ok(res.Ingredient) : UnprocessableEntity(res.Result.Errors);
        }

        private string? GetUserId() {
            return HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
