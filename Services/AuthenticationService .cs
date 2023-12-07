using Database.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using Services.DTOs;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IAuthenticationService
    {
        Task<String> AuthenticateAsync(LoginDTO model);
    }
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IConfiguration _configuration;

        public AuthenticationService(IUserRepository userRepository, IConfiguration configuration, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _roleRepository = roleRepository;
        }
        private async Task<string> PasswordHashing(string password, string salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), 10000))
            {
                byte[] hash = pbkdf2.GetBytes(20);
                return Convert.ToBase64String(hash);
            }
        }
        public async Task<String> AuthenticateAsync(LoginDTO model)
        {
            var user = await _userRepository.GetSingleByCondition(u => u.Email == model.Username || u.DisplayName == model.Username, includes: new[] { "Roles" });
            if (user.IsLockedout == false)
            {


                if (user == null)
                {
                    throw new Exception("Authentication failed. Please check your username and password");
                }

                var inputPassword = await PasswordHashing(model.Password, user.PasswordSalt);
                if (inputPassword != user.HashedPassword)
                {
                    throw new Exception("Authentication failed. Please check your username and password");
                }
                //var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);

                //if (!passwordValid)
                //{
                //    return null;
                //}
                var data = _roleRepository.GetAll().ToList();
                user.Roles = data;
                var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim("DisplayName", user.DisplayName ?? ""),

    };
                if (user.Roles != null)
                {
                    foreach (var use in user.Roles)
                    {
                        if (use != null && use.RoleTitle != null)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, use.RoleTitle));
                            claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", use.RoleTitle));

                        }
                    }

                }
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(30),
                    signingCredentials: creds);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            else
            {
                throw new Exception("Authentication failed. Please check your status");
            }


        }

    }

}