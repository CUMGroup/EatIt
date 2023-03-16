using EatIt.Core.Common.Configuration;
using EatIt.Core.Common.DTO;
using EatIt.Core.Common.DTO.Auth;
using EatIt.Core.Common.Interfaces;
using EatIt.Core.Models.Atomic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace EatIt.Core.Common.Services {
    internal class AuthService : IAuthService {

        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IApplicationDbContext _dbContext;

        public AuthService(RoleManager<Role> _roleManager, UserManager<User> _userManager, IConfiguration _configuration, IApplicationDbContext _dbContext) {
            this._roleManager = _roleManager;
            this._userManager = _userManager;
            this._configuration = _configuration;
            this._dbContext = _dbContext;
        }

        public async Task CreateRolesAsync<TRoles>() {
            var roles = typeof(TRoles).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(f => f.IsLiteral && !f.IsInitOnly && (f.FieldType == typeof(string) || f.FieldType == typeof(String)) && f.GetRawConstantValue() != null)
                .Select(x => (string)(x.GetRawConstantValue() ?? ""))
                .ToList();

            foreach (string r in roles) {
                if (!(await _roleManager.RoleExistsAsync(r))) {
                    await _roleManager.CreateAsync(new Role(r));
                }
            }
        }

        public async Task<LoginResultApiModel> LoginUserAsync(LoginApiModel loginModel) {

            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginModel.Password))
                return new LoginResultApiModel();

            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim> {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles) {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddYears(2),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return new LoginResultApiModel(Result.Success(), new JwtSecurityTokenHandler().WriteToken(token), token.ValidTo, user.UserName, user.Email);
        }

        public async Task<LoginResultApiModel> RegisterUserAsync(RegisterApiModel registerModel, string role) {

            var userExists = await _userManager.FindByEmailAsync(registerModel.Email);
            if (userExists != null) {
                return new LoginResultApiModel(Result.Failure("Account already exists!"));
            }

            User user = new() {
                Email = registerModel.Email,
                UserName = registerModel.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            // Create User
            var result = await _userManager.CreateAsync(user, registerModel.Password);
            if (!result.Succeeded)
                return new LoginResultApiModel(Result.Failure(result.Errors.Select(e => e.Description).ToList()));
            
            // Create Shopping List for user
            _dbContext.ShoppingLists.Add(new ShoppingList { User = user });
            await _dbContext.SaveChangesAsync();
            

            if (!await _roleManager.RoleExistsAsync(role))
                await CreateRolesAsync<UserRoles>();
            if(!await _roleManager.RoleExistsAsync(role)) {
                return new LoginResultApiModel(Result.Failure("Failed to add to Role. Account was created without it."));
            }

            await _userManager.AddToRoleAsync(user, role);

            return await LoginUserAsync(new LoginApiModel { Email = registerModel.Email, Password = registerModel.Password });
        }
    }
}
