using EatIt.Core.Common.DTO.Ingredient;
using EatIt.Core.Models.Joined;

namespace EatIt.Core.Common.DTO.Shopping {
    public class ShoppingIngredientModel {

        public double Amount { get; set; }
        public string Unit { get; set; }

        public IngredientModel Ingredient { get; set; }

        public static ShoppingIngredientModel MapFromShoppingIngredient(ShoppingIngredient ing) {
            return new ShoppingIngredientModel {
                Amount = ing.Amount,
                Ingredient = IngredientModel.MapFromIngredient(ing.Ingredient)
            };
        }

        public static ShoppingIngredient? MapToShoppingIngredient(ShoppingIngredientModel ingM) {
            if (ingM.Ingredient.Id == null)
                return null;
            return new ShoppingIngredient {
                Amount = ingM.Amount,
                IngredientsId = (Guid)ingM.Ingredient.Id
            };
        }
    }
}
