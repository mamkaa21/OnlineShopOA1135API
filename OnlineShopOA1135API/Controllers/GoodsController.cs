using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace OnlineShopOA1135API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoodsController : ControllerBase
    {
        readonly OnlineShopOa1135Context context;
        public GoodsController(OnlineShopOa1135Context context)
        {
            this.context = context;
        }

        [HttpPost("FindGoods")] //поиск товаров надо сделать
        public async Task<ActionResult> FindGoods(Good good)
        {
            return Ok();
        }
        //хз надо ил тут но нужна фильтрация




        

    }
}
