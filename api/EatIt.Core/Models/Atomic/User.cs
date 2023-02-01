
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EatIt.Core.Models.Atomic {
    public class User : IdentityUser<Guid> {

        public ICollection<Recipe> RecipesCreated { get; set; }

        public ICollection<Recipe> RecipesStarred { get;set; }

        public ICollection<WeeklyPlan> WeeklyPlans { get; set;}

        [Required]
        public ShoppingList ShoppingList { get; set; }

    }
}
