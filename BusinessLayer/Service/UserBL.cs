using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Interface;
using BusinessLayer.Interface;
using RepositoryLayer.Entity;
using ModelLayer.Model;

namespace BusinessLayer.Service
{
   public class UserBL: IUserBL
    {
        private readonly IUserRL _userRL;

        public UserBL(IUserRL userRL)
        {
            _userRL = userRL;
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

            // string token = GenerateToken(result.Data);

            return result;
        }

    }
}


