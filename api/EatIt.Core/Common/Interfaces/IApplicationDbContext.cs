using EatIt.Core.Models.Atomic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EatIt.Core.Common.Interfaces {
    public interface IApplicationDbContext {

        public DbSet<Ingredient> Ingredients { get; set;}
        public DbSet<IngredientCategory> IngredientCategories { get;set;}
        public DbSet<Recipe> Recipes { get; set;}
        public DbSet<RecipeCategory> RecipeCategories { get; set;}
        public DbSet<ShoppingList> ShoppingLists { get; set;}
        public DbSet<WeeklyPlan> WeeklyPlans { get; set;}

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<int> SaveChangesAsync();

        Task<bool> EnsureDatabaseAsync();
        Task MigrateDatabaseAsync();
    }
}
