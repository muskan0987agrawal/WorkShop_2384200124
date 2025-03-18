using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace RepositoryLayer.Service
{
   public class UserRL:IUserRL
    {
        private readonly MyAddressBookDbContext addressBookContext;
        public UserRL(MyAddressBookDbContext addressBookContext)
        {
            this.addressBookContext = addressBookContext;
        }
        public ResponseModel<UserEntity> Register(UserRegisterRequestModel user)
        {
            var existingUser = addressBookContext.Users.FirstOrDefault(g => g.Email == user.Email);

            if (existingUser != null) return null;

            //var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
            var newUser = new UserEntity
            {
                Name = user.Name,
                Email = user.Email,
                IsAdmin = false,
                PasswordHash = user.Password
            };

            addressBookContext.Users.Add(newUser);
            addressBookContext.SaveChanges();
            return new ResponseModel<UserEntity>
            {
                Data = newUser,
                Success = true,
                Message = "User Registered Successfully.",
                StatusCode = 200 // OK
            };

        }

        public ResponseModel<UserEntity> Login(UserLoginRequestModel user)
        {
            var existingUser = addressBookContext.Users.FirstOrDefault(g => g.Email == user.Email);

            if (existingUser == null)
            {
                return null; // Invalid credentials
            }

            var existingUser1 = new UserEntity
            {
                UserId = existingUser.UserId,   // ID ko include karo
                Name = existingUser.Name,
                Email = existingUser.Email,
                IsAdmin = existingUser.IsAdmin
            };

            return new ResponseModel<UserEntity>
            {
                Data = existingUser1,
                Success = true,
                Message = "User Logged in Successfully.",
                StatusCode = 200 // OK
            };
        }
        //public void StoreOTP(int otp, string email)
        //{
        //    var user = addressBookContext.Users.FirstOrDefault(c => c.Email == email);
        //    user.Otp = otp;
        //    addressBookContext.SaveChanges();
        //}

    }
}
