
using EatIt.Core.Models.Atomic;
using System.ComponentModel.DataAnnotations;

namespace EatIt.Core.Common.DTO.Recipe {
    public class RecipeDetail {

        public Guid Id { get; set; }
        public string Title { get; set; }
        public RecipeCategory? Category { get; set; }
        public DateTime DateUpdated { get; set; }
        [Range(0,10)]
        public int Difficulty { get; set; }
        public TimeSpan WorkDuration { get; set; }
        public TimeSpan TotalDuration { get; set; }
        public string Description { get; set; }
        public IEnumerable<Ingredient> Ingredients { get; set;}
        
        public Guid AuthorId { get; set; }
        public string UserName { get; set; }


    }
}
