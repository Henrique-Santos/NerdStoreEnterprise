using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetDevPack.Security.Jwt.Core.Jwa;
using NSE.Identidade.API.Data;
using NSE.Identidade.API.Extensions;

namespace NSE.Identidade.API.Configurations
{
    public static class IdentityConfiguration
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var appTokenSettingsSection = configuration.GetSection("AppTokenSettings");
            services.Configure<AppTokenSettings>(appTokenSettingsSection);

            // Persistencia das chaves no BD
            services.AddJwksManager(options =>
            {
                options.Jws = Algorithm.Create(AlgorithmType.RSA, JwtType.Jws);
                options.Jwe = Algorithm.Create(AlgorithmType.RSA, JwtType.Jwe);
            })
            .PersistKeysToDatabaseStore<ApplicationDbContext>();

            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddErrorDescriber<IdentityMensagensPortugues>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}