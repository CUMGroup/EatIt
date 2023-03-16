using EatIt.Core.Common.DTO;
using EatIt.Core.Common.DTO.User;

namespace EatIt.Core.Common.Interfaces {
    public interface IUserService {
        Task<UserOverview?> GetUserOverviewAsync(Guid userId);

        Task<bool> IsInRoleAsync(Guid userId, string role);
    }
}
