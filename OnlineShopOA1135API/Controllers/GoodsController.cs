using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

     
    }
}
