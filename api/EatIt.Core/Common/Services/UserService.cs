using EatIt.Core.Common.DTO.User;
using EatIt.Core.Common.Interfaces;
using EatIt.Core.Models.Atomic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace EatIt.Core.Common.Services {
    internal class UserService : IUserService {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager) {
            _userManager = userManager;
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
    }
}
