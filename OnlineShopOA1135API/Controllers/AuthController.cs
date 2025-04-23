using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.Eventing.Reader;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineShopOA1135API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        readonly OnlineShopOa1135Context context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthController(OnlineShopOa1135Context context)
        {
            this.context = context;
          
        }

        public class AuthOptions
        {
            public const string ISSUER = "MyAuthServer"; // издатель токена
            public const string AUDIENCE = "MyAuthClient"; // потребитель токена
            const string KEY = "mysupersecret_secretsecretsecretkey!123";   // ключ для шифрации
            public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
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
                    string role = user.Role.Title;
                    int id = user.Id;

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                        new Claim(ClaimTypes.Role, role)
                    };
                    var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    //кладём полезную нагрузку
                    claims: claims,
                    //устанавливаем время жизни токена 2 минуты
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

                    string token = new JwtSecurityTokenHandler().WriteToken(jwt);

                    //return Ok(new ResponceTokenAndEmployee
                    //{
                    //    Token = token,
                    //    Role = role,
                    //    Employee = user
                    //});
                    return Ok((UserModel) user);
                }
            }
        }

        //public class ResponceTokenAndEmployee
        //{
        //    public string Token { get; set; }
        //    public string Role { get; set; }
        //    public User User { get; set; }
        //}


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
