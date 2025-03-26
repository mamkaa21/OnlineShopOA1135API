using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OnlineShopOA1135API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        readonly OnlineShopOa1135Context context;
        public UserController(OnlineShopOa1135Context context)
        {
            this.context = context;
        }

       //контроллер с добалвением товара в корзину + оставить отзыв? + получить инфу о поль-вателе


    }
}

