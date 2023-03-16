using EatIt.Core.Common.DTO;
using EatIt.Core.Common.DTO.Ingredient;
using EatIt.Core.Common.DTO.Recipe;
using EatIt.Core.Common.Interfaces;
using EatIt.Core.Models.Atomic;
using Microsoft.EntityFrameworkCore;

namespace EatIt.Core.Common.Services {
    internal class IngredientService : IIngredientService {

        private readonly IApplicationDbContext _dbContext;

        public IngredientService(IApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Ingredient>> GetAllIngredientsAsync() {
            return await _dbContext.Ingredients
                .ToListAsync();
        }

        public async Task<Ingredient?> GetIngredientByIdAsync(Guid id) {
            return await _dbContext.Ingredients.FindAsync(id);
        }

        public async Task<IEnumerable<Ingredient>> GetIngredientsContainingAsync(string name) {
            return await _dbContext.Ingredients
                .Where(e => e.Name.ToLower().Contains(name.ToLower()))
                .ToListAsync();
        }

        public async Task<IEnumerable<Ingredient>> GetIngredientsStartsWithAsync(string name) {
            return await _dbContext.Ingredients
                .Where(e => e.Name.ToLower().StartsWith(name.ToLower()))
                .ToListAsync();
        }

        public async Task<IEnumerable<Ingredient>> GetIngredientsWithCategoryAsync(IEnumerable<string> categories) {
            return await _dbContext.Ingredients
                .Include(e => e.Category)
                .Where(e => 
                    e.Category != null &&
                    categories
                    .Select(c => c.ToLower())
                    .Contains(e.Category.Name.ToLower()))
                .ToListAsync();
        }

        public async Task<IEnumerable<IngredientCategory>> GetAllIngredientCategoriesAsync() {
            return await _dbContext.IngredientCategories.ToListAsync();
        }
        public async Task<IEnumerable<IngredientCategory>> GetAllIngredientCategoriesStartsWithAsync(string name) {
            return await _dbContext.IngredientCategories.Where(e => e.Name.ToLower().StartsWith(name.ToLower())).ToListAsync();
        }

        public async Task<IngredientOperationResultModel> UpdateIngredientAsync(IngredientModel ingredient) {

            var resultIngredient = new Ingredient() {
                Name = ingredient.Name,
                PricePerUnit = ingredient.PricePerUnit,
                Unit = ingredient.Unit
            };
            
            // handle category
            var category = await _dbContext.IngredientCategories.Where(e => e.Name.Equals(ingredient.Category)).FirstOrDefaultAsync();
            if (ingredient.Category != null && category == null) {
                await _dbContext.IngredientCategories.AddAsync(category = new IngredientCategory { Name = ingredient.Category });
            }
            resultIngredient.Category = category;

            _dbContext.Ingredients.Update(resultIngredient);

            var updated = await _dbContext.SaveChangesAsync();
            return updated > 0 ?
                new IngredientOperationResultModel { Ingredient = resultIngredient, Result = Result.Success() }
                : new IngredientOperationResultModel { Result = Result.Failure("Failed to create Ingredient") };
        }

        public async Task<IngredientOperationResultModel> DeleteIngredientAsync(Guid id) {
            var ingredient = await _dbContext.Ingredients.FindAsync(id);

            int updated = 0;
            if (ingredient != null) {
                _dbContext.Ingredients.Remove(ingredient);
                updated = await _dbContext.SaveChangesAsync();
            }

            return updated > 0 ?
                new IngredientOperationResultModel { Ingredient = ingredient, Result = Result.Success() }
                : new IngredientOperationResultModel { Result = ingredient == null ? Result.Failure("Could not find the Recipe") : Result.Failure("Failed to remove Recipe") };
        }
    }
}
