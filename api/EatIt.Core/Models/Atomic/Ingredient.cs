using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EatIt.Core.Models.Atomic {
    public class Ingredient {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public double PricePerKg { get; set; }

        [ForeignKey("Category")]
        public Guid? CategoryId { get; set; }
        public IngredientCategory? Category { get; set; }

        // Many to many with custom RecipeIngredient join table
        public ICollection<Recipe> Recipes { get; set; }
    }
}
