using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EatIt.Core.Common.DTO.Shopping {
    public class RecipeToShoppingListModel {

        public Guid RecipeId { get; set; }
        public int AmountPeople { get; set; }
    }
}
