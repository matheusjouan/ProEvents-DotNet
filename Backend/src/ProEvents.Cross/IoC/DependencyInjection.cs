using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ProEvents.Domain.Identity;
using ProEvents.Infra.Context;
using ProEvents.Infra.Interface;
using ProEvents.Infra.Repositories;
using ProEvents.Service.Interfaces;
using ProEvents.Service.Mappings;
using ProEvents.Service.Services;

namespace ProEvents.Cross.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
    IConfiguration config)
    {

        #region ("Configuração do acesso ao B.D")
        var conectStr = config.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(conectStr,
            ServerVersion.AutoDetect(conectStr))
        );
        #endregion

        #region ("Configuração do Identity")
        // Adiciona IdenityCore baseado em User
        services.AddIdentityCore<User>(options =>
        {
            // Opcional - Algumas regras de validações de campos
            options.Password.RequireDigit = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 4;
        })
        // Adiciona a Role baseado na Entidade criado por nós
        .AddRoles<Role>()
        // Será baseado na entidade Role criado por nós
        .AddRoleManager<RoleManager<Role>>()
        // SigninManager será baseado na Entidade User criado por nós 
        .AddSignInManager<SignInManager<User>>()
        .AddRoleValidator<RoleValidator<Role>>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();
        #endregion

        #region ("Configuração da Autenticação - JWT")
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
            options.TokenValidationParameters = new TokenValidationParameters
            {
                // Indicando que é usado uma chave para criptogragar/descriptografar o token
                ValidateIssuerSigningKey = true,
                // A mesma chave p/ criptografar na geração do token é usada para descriptografar
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])),
                // Não foi usado Audiente nem Issuer na aplicação
                ValidateIssuer = false,
                ValidateAudience = false
            });
        #endregion

        #region ("Configuração da Injeção de Serviços")
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IEventRepository, EventRepository>();

        services.AddScoped<IBatchRepository, BatchRepository>();
        services.AddScoped<IBatchService, BatchService>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<ISpeakerRepository, SpeakerRepository>();
        services.AddScoped<ISpeakerService, SpeakerService>();

        services.AddScoped<ISocialNetworkRepository, SocialNetworkRepository>();
        services.AddScoped<ISocialNetworkService, SocialNetworkService>();

        services.AddScoped<ITokenService, TokenService>();

        services.AddAutoMapper(typeof(MappingProfile));
        #endregion

        return services;
    }
}
