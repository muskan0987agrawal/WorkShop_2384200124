using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interface
{
   public interface IUserRL
    {
        ResponseModel<UserEntity> Register(UserRegisterRequestModel user);
        ResponseModel<UserEntity> Login(UserLoginRequestModel user);
    }
}
