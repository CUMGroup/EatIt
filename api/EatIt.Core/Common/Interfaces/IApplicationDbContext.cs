using EatIt.Core.Models.Atomic;
using EatIt.Core.Models.Joined;
using Microsoft.EntityFrameworkCore;


namespace EatIt.Core.Common.Interfaces {
    public interface IApplicationDbContext {

        public DbSet<Ingredient> Ingredients { get; set;}
        public DbSet<IngredientCategory> IngredientCategories { get;set;}
        public DbSet<Recipe> Recipes { get; set;}
        public DbSet<RecipeCategory> RecipeCategories { get; set;}
        public DbSet<ShoppingList> ShoppingLists { get; set;}
        public DbSet<WeeklyPlan> WeeklyPlans { get; set;}

        public DbSet<RecipeIngredient> RecipeIngredientsJoin { get; set; }
        public DbSet<ShoppingIngredient> ShoppingIngredientsJoin { get; set;}

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<int> SaveChangesAsync();

        Task<bool> EnsureDatabaseAsync();
        Task MigrateDatabaseAsync();
    }
}
