using EatIt.Core.Models.Atomic;

namespace EatIt.Core.Common.DTO.Recipe {
    public class RecipeOperationResultModel {

        public EatIt.Core.Models.Atomic.Recipe? Recipe { get; set; }

        public Result Result { get; set; }
    }
}
