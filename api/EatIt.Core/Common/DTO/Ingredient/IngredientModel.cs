using System.ComponentModel.DataAnnotations;


namespace EatIt.Core.Common.DTO.Ingredient {
    public class IngredientModel {

        public Guid? Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public double PricePerKg { get; set; }

        public string? Category { get; set; }
    }
}
