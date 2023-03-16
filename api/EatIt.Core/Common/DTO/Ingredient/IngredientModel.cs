using EatIt.Core.Common.Enums;
using System.ComponentModel.DataAnnotations;


namespace EatIt.Core.Common.DTO.Ingredient {
    public class IngredientModel {

        public Guid? Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public double PricePerUnit { get; set; }

        [Required]
        public Units Unit { get; set; }

        public string? Category { get; set; }


        public static IngredientModel MapFromIngredient(EatIt.Core.Models.Atomic.Ingredient ing) {
            return new IngredientModel {
                Category = ing.Category?.ToString(),
                Id = ing.Id,
                Name = ing.Name,
                PricePerUnit = ing.PricePerUnit,
                Unit = ing.Unit
            };
        }
    }
}
