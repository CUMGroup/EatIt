using EatIt.Core.Common.Interfaces;
using EatIt.Core.Database;
using EatIt.Core.Models.Atomic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EatIt.Core {
    public static class DependencyInjection {

        public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration) {

            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlite(configuration.GetConnectionString("DefaultConnection"))
           );
            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

            services.AddIdentity<User, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
