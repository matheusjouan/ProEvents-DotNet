using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProEvents.Domain.Identity;
using ProEvents.Service.DTOs;
using ProEvents.Service.Interfaces;

namespace ProEvents.Service.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config,
            UserManager<User> userManager,
            IMapper mapper
            )
        {
            _config = config;
            _userManager = userManager;
            _mapper = mapper;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }
        public async Task<string> CreateToken(UserUpdateDTO userUpdateDto)
        {
            var user = _mapper.Map<User>(userUpdateDto);

            // Claims - Afirmações sobre a Identidade do Usuário
            var claims = new List<Claim>
            {
                // Identificado do usuário (ID)
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            // Obtem as roles do usuário para passar no Token
            var roles = await _userManager.GetRolesAsync(user);

            // Será adicionado nas claims as roles
            // Usado Select (tipo um for) pois um usuário pode ter várias role
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            // Gereando a assinatura digital, que utilizamos para assinar o token
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            // Montando a estrutura de descrição do Token a partir das Claims
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            // Manipulador de token Jwt
            var tokenHandler = new JwtSecurityTokenHandler();
            // Cria o token passando a description
            var token = tokenHandler.CreateToken(tokenDescription);

            // Retorna a inscrição do token no formato Jwt
            return tokenHandler.WriteToken(token);
        }
    }
}