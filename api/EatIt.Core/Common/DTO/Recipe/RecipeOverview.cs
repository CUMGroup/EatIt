using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EatIt.Core.Common.DTO.Recipe {
    public class RecipeOverview {

        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime DateUpdated { get; set; }
        [Range(0, 10)]
        public int Difficulty { get; set; }
        public TimeSpan WorkDuration { get; set; }
        public TimeSpan TotalDuration { get; set; }
    }
}
