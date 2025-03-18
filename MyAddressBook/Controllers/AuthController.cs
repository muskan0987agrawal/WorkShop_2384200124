using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace MyAddressBook.Controllers
{
    [ApiController]
    [Route("/api/[controller]/")]
    public class AuthController : ControllerBase
    {
        private readonly IUserBL userBL;

        public AuthController(IUserBL userBL)
        {
            this.userBL = userBL;

        }

        [HttpPost("register")]
        public IActionResult RegisterUser(UserRegisterRequestModel user)
        {
            var result = userBL.Register(user);
            if (result == null)
            {
                return BadRequest(new { Message = "User already exists" });
            }

            return Ok(result);
        }

        [HttpPost("login")]
        public IActionResult LoginUser(UserLoginRequestModel user)
        {
            var result = userBL.Login(user);
            if (result == null)
            {
                return BadRequest(new { message = "Invalid Credentials!" });
            }

            return Ok(result);
        }

    }
}
