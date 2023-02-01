using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EatIt.Core.Models.Atomic {
    public class WeeklyPlan {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey("User"), Required]
        public Guid UserId { get; set; }
        public User User { get; set; }

        [Required]
        public DateTime Date { get; set; }

        
        public ICollection<Recipe> Recipes { get; set; }
    }
}
