using EatIt.Core.Models.Atomic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EatIt.Core.Models.Joined {
    public class ShoppingIngredient {

        [Required]
        public double Amount { get; set; }

        [ForeignKey("ShoppingList")]
        public Guid ShoppingListId { get; set; }
        public ShoppingList ShoppingList { get; set; }

        [ForeignKey("Ingredient")]
        public Guid IngredientsId { get; set; }
        public Ingredient Ingredient { get; set; }
    }
}
