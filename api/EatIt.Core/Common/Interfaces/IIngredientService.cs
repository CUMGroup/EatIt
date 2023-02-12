using EatIt.Core.Common.DTO.Ingredient;
using EatIt.Core.Models.Atomic;

namespace EatIt.Core.Common.Interfaces {
    public interface IIngredientService {

        public Task<IEnumerable<Ingredient>> GetAllIngredientsAsync();
        public Task<IEnumerable<Ingredient>> GetIngredientsStartsWithAsync(string name);
        public Task<IEnumerable<Ingredient>> GetIngredientsContainingAsync(string name);
        public Task<IEnumerable<Ingredient>> GetIngredientsWithCategoryAsync(IEnumerable<string> categories);
        
        public Task<IngredientOperationResultModel> UpdateIngredientAsync(IngredientModel ingredient);
        public Task<IngredientOperationResultModel> DeleteIngredientAsync(Guid id);
    }
}
