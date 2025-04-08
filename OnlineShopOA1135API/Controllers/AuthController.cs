using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;

namespace OnlineShopOA1135API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        readonly OnlineShopOa1135Context context;
        public AuthController(OnlineShopOa1135Context context)
        {
            this.context = context;
        }

        [HttpPost("CheckAccountIsExist")]
        public async Task<ActionResult> CheckAccountIsExist(UserModel User) //добавить проверку еще на роль - если рольID = 1, это админ, если рольID = 2 -> это юзер
        {
            var newUser = new User { Id = User.Id, Login = User.Login, Password = User.Password, RoleId = User.RoleId };
            if (string.IsNullOrEmpty(newUser.Login) || string.IsNullOrEmpty(newUser.Password))
                return BadRequest("Логин или пароль не иожет быть пустым");

            var user = await context.Users.FirstOrDefaultAsync(s => s.Login == newUser.Login);
            if (user == null)
                return NotFound("Неверный логин");
            else
            {
                if (newUser.Password != user.Password)
                {
                    return NotFound("Неверный пароль");
                }
                else
                {                   
                    return Ok("Успешно!");
                }
            }
        }

        [HttpPost("AddNewUser")]
        public async Task<ActionResult> AddNewUser(UserModel User) 
        {
            var newUser = new User { Id = User.Id, Login = User.Login, Password = User.Password, RoleId = User.RoleId };
            if (string.IsNullOrEmpty(newUser.Login))
                return BadRequest("Вы не ввели логин");
            var check = await context.Users.FirstOrDefaultAsync(s => s.Login == newUser.Login);
            if (check == null)
            {
                newUser.RoleId = 2;
                context.Users.Add(newUser);
                await context.SaveChangesAsync();
                return Ok("Успешно!");
            }
            else
                return BadRequest("Такой аккаунт уже существует");
        }
    }
}
