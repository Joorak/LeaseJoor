
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;
using System.Text;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var identityDbProvider = configuration["IdentityDbProvider"] ?? "SqlServer";
            var connectionString = configuration[$"ConnectionStrings:{identityDbProvider}"] ?? configuration["ConnectionStrings:Identity"];

            switch (identityDbProvider.ToLower())
            {
                case "sqlite":
                    services.AddDbContext<IdentityDb>(options =>
                        options.UseSqlite(connectionString, b => b.MigrationsAssembly(typeof(IdentityDb).Assembly.FullName)));
                    break;
                case "memory":
                    services.AddDbContext<IdentityDb>(options => options.UseInMemoryDatabase("LeaseJoorDb"));
                    break;
                case "localdb":
                    services.AddDbContext<IdentityDb>(options =>
                        options.UseSqlServer(connectionString, b => b.MigrationsAssembly(typeof(IdentityDb).Assembly.FullName)));
                    break;
                case "sqlserver":
                    services.AddDbContext<IdentityDb>(options =>
                        options.UseSqlServer(connectionString, b => b.MigrationsAssembly(typeof(IdentityDb).Assembly.FullName)));
                    break;
                default:
                    throw new InvalidOperationException($"Unsupported database provider: {identityDbProvider}");
            }

            services.AddDbContext<ReportingDb>(options =>
                options.UseSqlServer(configuration["ConnectionStrings:Reporting"] ?? throw new InvalidOperationException("Reporting connection string is missing")));

            //services.AddScoped<IdentityDb>(provider => provider.GetRequiredService<IdentityDb>());

            services.AddTransient<IAccountService, AccountService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddTransient<IPersianCalendarService, PersianCalendarService>();
            services.AddTransient<IEmailService, EmailService>();
            //services.AddTransient<IdentityMappingService>();
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
                options.Password.RequiredUniqueChars = 1;
            })
            .AddEntityFrameworkStores<IdentityDb>()
            .AddDefaultTokenProviders();
            //var dbBuilder = services.AddIdentity<AppUser>(opt =>
            //{
            //    opt.Password.RequireDigit = false;
            //    opt.Password.RequireLowercase = false;
            //    opt.Password.RequireNonAlphanumeric = false;
            //    opt.Password.RequireUppercase = false;
            //    opt.Password.RequiredLength = 3;
            //    opt.Password.RequiredUniqueChars = 1;
            //    opt.User.RequireUniqueEmail = true;
            //    opt.ClaimsIdentity.UserNameClaimType = ClaimTypes.NameIdentifier;
            //    opt.SignIn.RequireConfirmedPhoneNumber = true;
            //})
            //.AddRoles<AppRole>()
            //.AddUserManager<UserManager<AppUser>>() 
            //.AddRoleManager<RoleManager<AppRole>>()
            //.AddEntityFrameworkStores<IdentityDb>()
            //.AddDefaultTokenProviders();



            

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("Jwt:SecretKey is missing")))
                    };
                });

            services.AddAuthorizationCore(config =>
            {
                config.AddPolicy(StringRoleResources.Admin, policy => policy.RequireRole(ClaimTypes.Role, StringRoleResources.Admin));
                config.AddPolicy(StringRoleResources.Customer, policy => policy.RequireRole(ClaimTypes.Role, StringRoleResources.Customer));
                config.AddPolicy(StringRoleResources.Supplier, policy => policy.RequireRole(ClaimTypes.Role, StringRoleResources.Supplier));
                config.AddPolicy(StringRoleResources.User, policy => policy.RequireRole(ClaimTypes.Role, StringRoleResources.User));
                config.AddPolicy(StringRoleResources.User, defaultPolicy => defaultPolicy.RequireRole(ClaimTypes.Role, StringRoleResources.Default));
            });

            return services;
        }
    }
}