
using EatIt.Core.Models.Atomic;
using EatIt.Core.Models.Joined;
using System.ComponentModel.DataAnnotations;

namespace EatIt.Core.Common.DTO.Recipe {
    public class RecipeDetail {

        public Guid? Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; }
        [Required]
        public string? Category { get; set; }
        public DateTime DateUpdated { get; set; }
        [Required, Range(0,10)]
        public int Difficulty { get; set; }
        [Required]
        public TimeSpan WorkDuration { get; set; }
        [Required]
        public TimeSpan TotalDuration { get; set; }
        [Required]
        public string Description { get; set; }
        public IEnumerable<RecipeIngredient> RecipeIngredients { get; set;}
        
        public Guid? AuthorId { get; set; }
        public string? UserName { get; set; }

    }
}
