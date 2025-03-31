using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace BusinessLayer.Interface
{
   public interface IUserBL
    {
        ResponseModel<UserEntity> Register(UserRegisterRequestModel user);
        ResponseModel<UserEntity> Login(UserLoginRequestModel user);

        Task<AsyncResponseModel<string>> ForgotPassword(ForgotPasswordRequestModel request);

        ResponseModel<string> ResetPassword(ResetPasswordRequestModel request);
    }
}
