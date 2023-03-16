using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EatIt.Core.Common.DTO.Shopping {
    public class ShoppingListResultModel {

        public Result Result { get; set; }

        public IEnumerable<ShoppingIngredientModel> Ingredients { get; set;}
    }
}
