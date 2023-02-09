using EatIt.Core.Common.DTO.Auth;

namespace EatIt.Core.Common.Interfaces
{
    public interface IAuthService {
        Task<LoginResultApiModel> RegisterUserAsync(RegisterApiModel registerModel, string role);

        Task<LoginResultApiModel> LoginUserAsync(LoginApiModel loginModel);

        Task CreateRolesAsync<TRoles>();
    }
}
