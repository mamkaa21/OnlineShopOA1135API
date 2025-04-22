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

        //[HttpPost("CreateOrder")]
        //  public async Task<ActionResult> CreateOrder(Order order) { } надо думать сука

        [HttpPut("GetRatingForGood")] //возможность поставить оценку товару??
        public async Task<ActionResult> GetRatingForGood(Good good)
        {
            try
            {
                context.Goods.Update(good); //типа обновляем оценку у товара хаха
                await context.SaveChangesAsync();
                return Ok("Успешно");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("CreateReviewByGood")]
        public async Task<ActionResult> CreateReviewByGood(Good good)
        {
            try
            {
                context.Goods.Update(good); //типа добавили и обновили товар - теперь у него есть отзыв
                await context.SaveChangesAsync();
                return Ok("Успешно");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //контроллер с добалвением товара в корзину + оставить отзыв? + получить инфу о поль-вателе


    }
}

