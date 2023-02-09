using EatIt.Core.Common.DTO;
using EatIt.Core.Common.Interfaces;
using EatIt.Core.Models.Atomic;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EatIt.Core.Common.Services {
    internal class UserService : IUserService {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager) {
            _userManager = userManager;
        }

        public async Task<User?> GetUserAsync(string userId) {
            var user = await _userManager.FindByIdAsync(userId);
            return user;
        }

        public async Task<bool> IsInRoleAsync(string userId, string role) {
            var user = await GetUserAsync(userId);
            if (user == null)
                return false;
            return await _userManager.IsInRoleAsync(user, role);
        }
    }
}
