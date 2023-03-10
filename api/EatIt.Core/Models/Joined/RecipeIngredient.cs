using EatIt.Core.Models.Atomic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EatIt.Core.Models.Joined {
    public class RecipeIngredient {

        [Required]
        public double AmountForTwo { get; set; }
        [Required, MaxLength(5)]
        public string Unit { get; set; }

        [ForeignKey("Recipe")]
        public Guid RecipesId { get; set; }
        public Recipe Recipe { get; set; }

        [ForeignKey("Ingredient")]
        public Guid IngredientsId { get; set; }
        public Ingredient Ingredient { get; set; }
    }
}
