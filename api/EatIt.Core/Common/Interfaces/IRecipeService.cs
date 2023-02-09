using EatIt.Core.Common.DTO.Recipe;
using EatIt.Core.Models.Atomic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EatIt.Core.Common.Interfaces {
    public interface IRecipeService {

        public Task<IEnumerable<RecipeOverview>?> GetUserStarredAsync(Guid userId);
        public Task<IEnumerable<RecipeOverview>?> GetUserCreationsAsync(Guid userId);

        public Task<RecipeDetail?> GetDetailedRecipeByIdAsync(Guid recipeId);
        public Task<IEnumerable<RecipeOverview>> GetRecipesByNameStartsWithAsync(string name);
        public Task<IEnumerable<RecipeOverview>> GetRecipesByNameContainsAsync(string name);
        public Task<IEnumerable<RecipeOverview>> GetRecipesByCategoryAsync(IEnumerable<RecipeCategory> categories);
        public Task<IEnumerable<RecipeOverview>> GetRecipesWithIngredientsAsync(IEnumerable<Ingredient> ingredients);

        public Task<RecipeOperationResultModel> UpdateRecipeAsync(RecipeDetail recipe);
        public Task<RecipeOperationResultModel> DeleteRecipeAsync(Guid recipeId);
    }
}
