using EatIt.Core.Common.DTO;
using EatIt.Core.Common.DTO.Ingredient;
using EatIt.Core.Common.DTO.Recipe;
using EatIt.Core.Common.DTO.Shopping;
using EatIt.Core.Common.DTO.User;
using EatIt.Core.Common.Interfaces;
using EatIt.Core.Models.Atomic;
using EatIt.Core.Models.Joined;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EatIt.Core.Common.Services {
    internal class UserService : IUserService {
        private readonly UserManager<User> _userManager;
        private readonly IApplicationDbContext _dbContext;

        public UserService(UserManager<User> userManager, IApplicationDbContext dbContext) {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<UserOverview?> GetUserOverviewAsync(Guid userId) {
            var user = await _userManager.Users
                .Where(e => e.Id.Equals(userId))
                .Include(e => e.RecipesCreated)
                .Include(e => e.RecipesStarred)
                .Include(e=>e.ShoppingList)
                .ThenInclude(s => s.Ingredients)
                .Select(e => new UserOverview() { 
                    Email=e.Email,
                    UserName = e.UserName,
                    CreateCount=e.RecipesCreated.Count,
                    FavCount=e.RecipesStarred.Count,
                    ShoppingListCount=e.ShoppingList.Ingredients.Count
                })
                .FirstOrDefaultAsync();
            return user;
        }

        public async Task<bool> IsInRoleAsync(Guid userId, string role) {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return false;
            return await _userManager.IsInRoleAsync(user, role);
        }


        public async Task<ShoppingListResultModel> AddIngredientsToShoppingListAsync(Guid userId, IEnumerable<ShoppingIngredientModel> ingredients) {
            var list = await _dbContext.ShoppingLists
                .Where(e => e.UserId.Equals(userId))
                .Include(e => e.Ingredients)
                .ThenInclude(e => e.Ingredient)
                .FirstOrDefaultAsync();
            if (list == null)
                return new ShoppingListResultModel { Result = Result.Failure("Error retrieving user Shopping List") };

            foreach (var ingM in ingredients) {
                if (ingM == null || ingM.Ingredient.Id == null)
                    return new ShoppingListResultModel { Result = Result.Failure("Some ingredients had no Id") };
                var ing = list.Ingredients.Where(e => e.IngredientsId.Equals(ingM.Ingredient.Id)).FirstOrDefault();
                if(ing == null) {
                    ing = new ShoppingIngredient {
                        Amount = ingM.Amount,
                        IngredientsId = (Guid)ingM.Ingredient.Id,
                        ShoppingListId = list.UserId
                    };
                    await _dbContext.ShoppingIngredientsJoin.AddAsync(ing);
                }else {
                    ing.Amount += ingM.Amount;
                    _dbContext.ShoppingIngredientsJoin.Update(ing);
                }

            }

            var updated = await _dbContext.SaveChangesAsync();

            return updated > 0 ?
                new ShoppingListResultModel {
                    Result = Result.Success(),
                    Ingredients = list.Ingredients.Select(ShoppingIngredientModel.MapFromShoppingIngredient)
                } :
                new ShoppingListResultModel {
                    Result = Result.Failure("Error adding ingredients to Shopping List")
                };
        }

        public async Task<ShoppingListResultModel> AddRecipeIngredientsToShoppingListAsync(Guid userId, RecipeToShoppingListModel recipeShoppingModel) {
            var list = await _dbContext.ShoppingLists
                .Where(e => e.UserId.Equals(userId))
                .Include(e => e.Ingredients)
                .ThenInclude(e => e.Ingredient)
                .FirstOrDefaultAsync();
            if (list == null)
                return new ShoppingListResultModel { Result = Result.Failure("Error retrieving user Shopping List") };

            var recIngList = await _dbContext.RecipeIngredientsJoin
                .Where(e => e.RecipeId.Equals(recipeShoppingModel.RecipeId))
                .ToListAsync();

            foreach(var recIng in recIngList) {
                if(recIng == null)
                    return new ShoppingListResultModel { Result = Result.Failure("Some ingredients had no Id") };

                var ing = list.Ingredients.Where(e => e.IngredientsId.Equals(recIng?.IngredientId)).FirstOrDefault();
                if (ing == null) {
                    ing = new ShoppingIngredient {
                        Amount = recIng.AmountForTwo / 2.0 * recipeShoppingModel.AmountPeople,
                        IngredientsId = recIng.IngredientId,
                        ShoppingListId = list.UserId
                    };
                    await _dbContext.ShoppingIngredientsJoin.AddAsync(ing);
                }else {
                    ing.Amount += recIng.AmountForTwo / 2.0 * recipeShoppingModel.AmountPeople;
                    _dbContext.ShoppingIngredientsJoin.Update(ing);
                }
            }
            var updated = await _dbContext.SaveChangesAsync();

            return updated > 0 ?
                new ShoppingListResultModel {
                    Result = Result.Success(),
                    Ingredients = list.Ingredients.Select(ShoppingIngredientModel.MapFromShoppingIngredient)
                } :
                new ShoppingListResultModel {
                    Result = Result.Failure("Failed to add ingredients to Shopping List")
                };
        }

        public async Task<ShoppingListResultModel> ClearShoppingListAsync(Guid userId) {
            var list = await _dbContext.ShoppingLists
                .Where(e => e.UserId.Equals(userId))
                .Include(e => e.Ingredients)
                .FirstOrDefaultAsync();
            if (list == null)
                return new ShoppingListResultModel { Result = Result.Failure("Error retrieving user Shopping List") };
            list.Ingredients.Clear();

            var updated = await _dbContext.SaveChangesAsync();
            return updated > 0 ?
                new ShoppingListResultModel {
                    Result = Result.Success(),
                    Ingredients = list.Ingredients.Select(ShoppingIngredientModel.MapFromShoppingIngredient)
                } :
                new ShoppingListResultModel { Result = Result.Failure("Could not clear the Shopping List!") };
        }

        public async Task<ShoppingList?> GetShoppingListAsync(Guid userId) {
            return await _dbContext.ShoppingLists
                .Where(e => e.UserId.Equals(userId))
                .Include (e => e.Ingredients)
                .ThenInclude(e => e.Ingredient)
                .FirstOrDefaultAsync();
        }

        public async Task<ShoppingListResultModel> RemoveFromShoppingListAsync(Guid userId, ShoppingIngredientModel ingredient) {
            if(ingredient.Ingredient.Id == null)
                return new ShoppingListResultModel { Result = Result.Failure("Ingredient Id is null") };

            var list = await _dbContext.ShoppingIngredientsJoin
                .Include(e => e.ShoppingList)
                .Where(e => e.ShoppingList.UserId.Equals(userId))
                .Where(e => e.IngredientsId.Equals(ingredient.Ingredient.Id))
                .ToListAsync();
            _dbContext.ShoppingIngredientsJoin.RemoveRange(list);

            var updated = await _dbContext.SaveChangesAsync();

            if (updated > 0) {
                var newShop = await _dbContext.ShoppingLists
                    .Where(e => e.UserId.Equals(userId))
                    .Include(e => e.Ingredients)
                    .ThenInclude(e => e.Ingredient)
                    .FirstOrDefaultAsync();
                if(newShop == null) {
                    return new ShoppingListResultModel {
                        Result = Result.Failure("Error after deleting")
                    };
                }
                return new ShoppingListResultModel {
                    Result = Result.Success(),
                    Ingredients = newShop.Ingredients.Select(ShoppingIngredientModel.MapFromShoppingIngredient)
                };
            }

            return new ShoppingListResultModel {
                Result = Result.Failure("Failed to delete Ingredients from Shopping List")
            };
        }
    }
}
