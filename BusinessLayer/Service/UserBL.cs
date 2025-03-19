using RepositoryLayer.Interface;
using BusinessLayer.Interface;
using RepositoryLayer.Entity;
using ModelLayer.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace BusinessLayer.Service
{
    public class UserBL : IUserBL
    {
        private readonly IUserRL _userRL;
        private readonly IConfiguration _configuration;


        public UserBL(IUserRL userRL, IConfiguration configuration)
        {
            _userRL = userRL;
            _configuration = configuration;
        }

        public ResponseModel<UserEntity> Register(UserRegisterRequestModel user)
        {
            return _userRL.Register(user);
        }

        public ResponseModel<UserEntity> Login(UserLoginRequestModel user)
        {
            var result = _userRL.Login(user);

            if (result == null || result.Data == null)
            {
                return null;
            }

            string token = GenerateToken(result.Data);

            return new ResponseModel<UserEntity>
            {
                Data = result.Data,
                Message = "Login successful",
                StatusCode = 200,
                Success = true,
                Token = token
            };

            return result;
        }

        public string GenerateToken(UserEntity user)
        {
            if (string.IsNullOrEmpty(_configuration["Jwt:Key"]) ||
                string.IsNullOrEmpty(_configuration["Jwt:Issuer"]) ||
                string.IsNullOrEmpty(_configuration["Jwt:Audience"]))
            {
                throw new InvalidOperationException("JWT Configuration values are missing");
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"] ?? "AddressBookAPI"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserId", user.UserId.ToString()),
                new Claim("Email", user.Email)
            };

            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var signin = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                //claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: signin
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

