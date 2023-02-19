using EatIt.Core.Common.Interfaces;
using EatIt.Core.Models.Atomic;
using EatIt.Core.Models.Joined;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EatIt.Core.Database {
    internal class ApplicationDbContext : IdentityDbContext<User, Role, Guid>, IApplicationDbContext {
        public DbSet<Ingredient> Ingredients { get ; set ; }
        public DbSet<IngredientCategory> IngredientCategories { get ; set ; }
        public DbSet<Recipe> Recipes { get ; set ; }
        public DbSet<RecipeCategory> RecipeCategories { get ; set ; }
        public DbSet<ShoppingList> ShoppingLists { get ; set ; }
        public DbSet<WeeklyPlan> WeeklyPlans { get ; set ; }

        public DbSet<RecipeIngredient> RecipeIngredientsJoin { get ; set ; }
        public DbSet<ShoppingIngredient> ShoppingIngredientsJoin { get ; set ; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {

        }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
            #region RecipeIngredient Many to many
            builder.Entity<Recipe>()
                .HasMany(x => x.RecipeIngredients)
                .WithOne();
            //.UsingEntity<RecipeIngredient>();

            builder.Entity<RecipeIngredient>()
                .HasOne(x => x.Ingredient)
                .WithMany();

            builder.Entity<RecipeIngredient>()
                .HasKey(x => new { x.RecipeId, x.IngredientId });
           /* builder.Entity<RecipeIngredient>()
                .HasOne(x => x.Ingredient)
                .WithOne()
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<RecipeIngredient>()
                .HasOne(x => x.Recipe)
                .WithOne()
                .OnDelete(DeleteBehavior.NoAction);*/
            #endregion

            #region ShoppingIngredient Many to many
            builder.Entity<ShoppingList>()
                .HasMany(x => x.Ingredients)
                .WithOne();

            builder.Entity<ShoppingIngredient>()
                .HasOne(x => x.Ingredient)
                .WithMany();

            builder.Entity<ShoppingIngredient>()
                .HasKey(x => new { x.ShoppingListId, x.IngredientsId });
            /*builder.Entity<ShoppingList>()
                .HasMany(x => x.Ingredients)
                .WithMany()
                .UsingEntity<ShoppingIngredient>();

            builder.Entity<ShoppingIngredient>()
                .HasOne(x => x.Ingredient)
                .WithOne()
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<ShoppingIngredient>()
                .HasOne(x => x.ShoppingList)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);*/
            #endregion

            #region WeeklyPlanRecipe Many to many
            builder.Entity<WeeklyPlan>()
                .HasMany(e => e.Recipes)
                .WithMany()
                .UsingEntity<WeeklyPlanRecipe>();

            builder.Entity<WeeklyPlanRecipe>()
                .HasOne(e => e.Recipe)
                .WithOne()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<WeeklyPlanRecipe>()
                .HasOne(e => e.WeeklyPlan)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
            #endregion


            // User created relation
            builder.Entity<Recipe>()
                .HasOne(e => e.Author)
                .WithMany(e => e.RecipesCreated)
                .HasForeignKey(e => e.AuthorId);

            // user starred relation
            builder.Entity<User>()
                .HasMany(e => e.RecipesStarred)
                .WithMany()
                .UsingEntity(join => join.ToTable("RecipesStarredUser"));

            // user weekly plan relation
            builder.Entity<User>()
                .HasMany(e => e.WeeklyPlans)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId);

           




        }

        public Task<bool> EnsureDatabaseAsync() {
            return Database.EnsureCreatedAsync();
        }

        public Task MigrateDatabaseAsync() {
            return Database.MigrateAsync();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken) {
            return base.SaveChangesAsync(cancellationToken);
        }

        public Task<int> SaveChangesAsync() {
            return base.SaveChangesAsync();
        }
    }
}
