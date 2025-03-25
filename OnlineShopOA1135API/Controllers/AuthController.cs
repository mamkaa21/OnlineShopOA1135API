using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OnlineShopOA1135API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        readonly OnlineShopOa1135Context onlineShopOa1135Context;
        public AuthController(OnlineShopOa1135Context onlineShopOa1135Context)
        {
            this.onlineShopOa1135Context = onlineShopOa1135Context;
        }

        [HttpPost("CheckAccountIsExist")]
        public async Task<ActionResult> CheckAccountIsExist(User user)
        {
            var newUser = new User();
            if (string.IsNullOrEmpty(newUser.Login) || string.IsNullOrEmpty(newUser.Password))
            return BadRequest("Логин или пароль не иожет быть пустым");

            var user = await 
        }
    }
}
