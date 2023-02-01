using EatIt.Core.Models.Joined;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EatIt.Core.Models.Atomic {
    
    public class Recipe {

        [Key ,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; }

        [ForeignKey("Category")]
        public Guid? CategoryId { get; set; }
        public RecipeCategory? Category { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateUpdated { get; set; }

        [Required, Range(0,10)]
        public int Difficulty { get; set; }

        [Required]
        public TimeSpan WorkDuration { get; set; }
        [Required]
        public TimeSpan TotalDuration { get; set; }

        // Markdown description
        [Required]
        public string Description { get; set; }

        // Many to many with custom RecipeIngredient join table
        public ICollection<Ingredient> Ingredients { get; set; }

        [ForeignKey("Author"), Required]
        public Guid AuthorId { get; set; }
        public User Author { get; set; }

    }
}
