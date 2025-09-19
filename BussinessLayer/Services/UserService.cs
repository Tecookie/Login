using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BussinessLayer.IServices;
using DataAccessLayer.DTO;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace BussinessLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<string> Login(LoginDTO dto)
        {
            var secretKey = "the-key-size-must-be-greater-than-512-bits-to-make-a-secret-key-random-word-random-word"; //Cái này thường sẽ dấu ở UserSecret
            var secretKeyByte = Encoding.UTF8.GetBytes(secretKey);
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            string role = "Admin";
            string status = "Hoạt động";
            string fullName = "Dương Hồng Quang";
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
               new Claim("UserName", fullName),
                new Claim("Role", role),
                new Claim("Status", status)
           }),

                Expires = DateTime.UtcNow.AddMinutes(180),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyByte),
    SecurityAlgorithms.HmacSha512Signature)
            };
            var principal = new ClaimsPrincipal(tokenDescription.Subject);
            _httpContextAccessor.HttpContext.User = principal;
            var token = jwtTokenHandler.CreateToken(tokenDescription);
            return jwtTokenHandler.WriteToken(token);
        }
        public async Task<string> ValidateGoogleToken(string token)
        {
            var GoogleClientId = "GoogleId";


            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(token, new GoogleJsonWebSignature.ValidationSettings
                //Install package Google.Apis.Auth
                {
                    Audience = new[] { GoogleClientId }
                });
                string email = payload.Email;
                return email;
                //Có email rồi thì kéo database check User có Email tương ứng

            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }

    }
}
