using EatIt.Core.Common.DTO;
using EatIt.Core.Models.Atomic;

namespace EatIt.Core.Common.Interfaces {
    public interface IUserService {
        Task<User> GetUserAsync(string userId);

        Task<bool> IsInRoleAsync(string userId, string role);
    }
}
