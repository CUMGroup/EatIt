using EatIt.Core.Common.DTO;
using EatIt.Core.Common.DTO.Recipe;
using EatIt.Core.Common.DTO.Shopping;
using EatIt.Core.Common.DTO.User;
using EatIt.Core.Models.Atomic;

namespace EatIt.Core.Common.Interfaces {
    public interface IUserService {
        Task<UserOverview?> GetUserOverviewAsync(Guid userId);

        Task<bool> IsInRoleAsync(Guid userId, string role);

        Task<ShoppingList?> GetShoppingListAsync(Guid userId);

        Task<ShoppingListResultModel> AddIngredientsToShoppingListAsync(Guid userId, IEnumerable<ShoppingIngredientModel> ingredients);

        Task<ShoppingListResultModel> AddRecipeIngredientsToShoppingListAsync(Guid userId, RecipeToShoppingListModel recipe);

        Task<ShoppingListResultModel> RemoveFromShoppingListAsync(Guid userId, ShoppingIngredientModel ingredient);

        Task<ShoppingListResultModel> ClearShoppingListAsync(Guid userId);
    }
}
