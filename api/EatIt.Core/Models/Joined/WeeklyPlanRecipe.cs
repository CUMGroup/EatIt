using EatIt.Core.Models.Atomic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EatIt.Core.Models.Joined {
    public class WeeklyPlanRecipe {


        [ForeignKey("Recipe")]
        public Guid RecipesId { get; set; }
        public Recipe Recipe { get; set; }

        [ForeignKey("WeeklyPlan")]
        public Guid WeeklyPlanId { get; set; }
        public WeeklyPlan WeeklyPlan { get; set; }
    }
}
