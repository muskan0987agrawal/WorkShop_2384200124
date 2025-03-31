using System.Text.RegularExpressions;
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
        private readonly IEmailSender _emailSender;
      //  private readonly 

        public UserBL(IUserRL userRL, IConfiguration configuration, IEmailSender emailSender)
        {
            _userRL = userRL;
            _configuration = configuration;
            _emailSender = emailSender;
        }

        public ResponseModel<UserEntity> Register(UserRegisterRequestModel user)
        {
            // Validate Name (Only letters, 3 to 50 characters)
            if (string.IsNullOrWhiteSpace(user.Name) || !Regex.IsMatch(user.Name, @"^[A-Za-z\s]{3,50}$"))
            {
                return new ResponseModel<UserEntity>
                {
                    Data = null,
                    Message = "Invalid name. Name should only contain letters and be 3 to 50 characters long.",
                    StatusCode = 400,
                    Success = false
                };
            }

            // Validate Email (Standard email pattern)
            if (string.IsNullOrWhiteSpace(user.Email) || !Regex.IsMatch(user.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                return new ResponseModel<UserEntity>
                {
                    Data = null,
                    Message = "Invalid email format.",
                    StatusCode = 400,
                    Success = false
                };
            }

            // Validate Password (Minimum 6 characters, at least one uppercase, one lowercase, one number, and one special character)
            if (string.IsNullOrWhiteSpace(user.Password) ||
                !Regex.IsMatch(user.Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$"))
            {
                return new ResponseModel<UserEntity>
                {
                    Data = null,
                    Message = "Invalid password. Password must be at least 6 characters long and include at least one uppercase letter, one lowercase letter, one number, and one special character.",
                    StatusCode = 400,
                    Success = false
                };
            }

            // If all validations pass, call the Repository Layer
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
                claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: signin
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<AsyncResponseModel<string>> ForgotPassword(ForgotPasswordRequestModel request)
        {
            Random random = new Random();
            int otp = random.Next(100000, 999999);
            var subject = "Password Reset Request";
            var message = $"To reset your password, please use the OTP below:\n\n{otp}";

            // Send email without including Action in response
            await _emailSender.SendEmailAsync(request.Email, subject, message);

            _userRL.StoreOTP(otp, request.Email);
            return new AsyncResponseModel<string>(
            "Your OTP is the token",
            "Email sent successfully",
            200,
            true,
            "Check your email"
            );
        }

        public ResponseModel<string> ResetPassword(ResetPasswordRequestModel request)
        {
            return _userRL.ResetPassword(request);
        }
    }
}


