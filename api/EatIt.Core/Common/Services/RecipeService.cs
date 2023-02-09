
using EatIt.Core.Common.DTO.Recipe;
using EatIt.Core.Common.Interfaces;
using EatIt.Core.Models.Atomic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EatIt.Core.Common.Services {
    internal class RecipeService : IRecipeService{

        private readonly IApplicationDbContext _dbContext;
        private readonly UserManager<User> _userManager;

        public RecipeService(IApplicationDbContext _dbContext, UserManager<User> _userManager) {
            this._dbContext = _dbContext;
            this._userManager= _userManager;
        }

        public async Task<IEnumerable<RecipeOverview>?> GetUserStarredAsync(Guid userId) {
            return await _userManager.Users
                .Where(e => e.Id.Equals(userId))
                .Include(e => e.RecipesStarred)
                .SelectMany(e => e.RecipesStarred)
                .Select(e => MapRecipeToOverview(e))
                .ToListAsync();
        }
        public async Task<IEnumerable<RecipeOverview>?> GetUserCreationsAsync(Guid userId) {
            return await _userManager.Users
                .Where(e => e.Id.Equals(userId))
                .Include(e => e.RecipesCreated)
                .SelectMany(e => e.RecipesCreated)
                .Select(e => MapRecipeToOverview(e))
                .ToListAsync();
        }

        public Task<RecipeDetail?> GetDetailedRecipeByIdAsync(Guid recipeId) {
            return _dbContext.Recipes
                .Where(e => e.Id.Equals(recipeId))
                .Include(e => e.Category)
                .Include(e => e.Ingredients)
                .Include(e => e.Author)
                .Select(e => MapRecipeToDetail(e))
                .FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<RecipeOverview>> GetRecipesByNameStartsWithAsync(string name) {
            return await _dbContext.Recipes
                .Where(e => e.Title.StartsWith(name))
                .Select(e => MapRecipeToOverview(e))
                .ToListAsync();
        }
        public async Task<IEnumerable<RecipeOverview>> GetRecipesByNameContainsAsync(string name) {
            return await _dbContext.Recipes
                .Where(e => e.Title.Contains(name))
                .Select(e => MapRecipeToOverview(e))
                .ToListAsync();
        }
        public async Task<IEnumerable<RecipeOverview>> GetRecipesByCategoryAsync(IEnumerable<RecipeCategory> categories) {
            return await _dbContext.Recipes
                .Include(e => e.Category)
                .Where(e => categories.Contains(e.Category))
                .Select(e => MapRecipeToOverview(e))
                .ToListAsync();
        }
        public async Task<IEnumerable<RecipeOverview>> GetRecipesWithIngredientsAsync(IEnumerable<Ingredient> ingredients) {
            return await _dbContext.Recipes
                .Include(e => e.Ingredients)
                .Where(e => e.Ingredients.Intersect(ingredients).Any())
                .Select(e => MapRecipeToOverview(e))
                .ToListAsync();
        }



        private RecipeOverview MapRecipeToOverview(Recipe e) {
            return new RecipeOverview {
                Id = e.Id,
                Title = e.Title,
                DateUpdated = e.DateUpdated,
                Difficulty = e.Difficulty,
                TotalDuration = e.TotalDuration,
                WorkDuration = e.WorkDuration
            };
        }

        private RecipeDetail MapRecipeToDetail(Recipe e) {
            return new RecipeDetail {
                Id = e.Id,
                Title = e.Title,
                Category = e.Category,
                DateUpdated = e.DateUpdated,
                Difficulty = e.Difficulty,
                WorkDuration = e.WorkDuration,
                TotalDuration = e.TotalDuration,
                Description = e.Description,
                Ingredients = e.Ingredients,
                AuthorId = e.AuthorId,
                UserName = e.Author.UserName
            };
        }

    }
}
