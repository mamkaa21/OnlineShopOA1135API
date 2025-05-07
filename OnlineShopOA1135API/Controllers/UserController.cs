using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.SymbolStore;
using System.Security.Claims;

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

        [HttpPost("CreateOrder")]
        public async Task<ActionResult> AddToBasket([FromBody] AddToCartRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized("error");
            }

            Order activeOrder = await context.Orders.FirstOrDefaultAsync(o => o.UserId == userId);

            if (activeOrder == null)
            {
                activeOrder = new Order
                {
                    UserId = userId,
                    DateCreated = DateTime.UtcNow,
                    Status = "корзина"

                };
                context.Orders.Add(activeOrder);
                await context.SaveChangesAsync();
            }

            //Проверяем, есть ли уже этот товар в корзине
            OrderGoodsCross existingItem = await context.OrderGoodsCrosses
                .FirstOrDefaultAsync(og => og.OrderId == activeOrder.Id && og.GoodsId == request.GoodId);

            if (existingItem != null)
            {
                existingItem.Quantity += request.Quantity;
                context.Update(existingItem);
            }
            else
            {
                // Если товара нет, добавляем новую запись
                var orderGoodsCross = new OrderGoodsCross
                {
                    OrderId = activeOrder.Id,
                    GoodsId = request.GoodId,
                    Quantity = request.Quantity
                };
                context.OrderGoodsCrosses.Add(orderGoodsCross);
            }

            await context.SaveChangesAsync();

            return Ok("Товар добавлен в корзину.");
        }


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

        [HttpGet]
        public async Task<ActionResult> GetUserData()
        {
            var id = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return Ok(context.Users.Find(id));
        }

        [HttpGet("GetOrderBasket")]
        public async Task<List<Order>> GetOrder()
        {
            await Task.Delay(10);
            var order = context.Orders.ToList();

            return order;
        }

        [HttpGet("GetOrderBasketCount/{userId}")]
        public async Task<int?> GetCount(int userId)
        {
            await Task.Delay(10);
            var order = context.Orders.FirstOrDefaultAsync(s => s.UserId == userId && s.Status == "корзина");
            var cr = context.OrderGoodsCrosses.Where(s => s.OrderId == order.Id);
            int? count = 0;
            foreach(var g in cr)
            {
                count += g.Quantity;
            }
            return count;
        }


        [HttpPost("FiltGoodsByCat")] //фильтрация 
        public async Task<ActionResult<IEnumerable<Good>>> FiltGoodsByCat([FromBody] List<int?> categoryIds)
        {
            IQueryable<Good> query = context.Goods.Include(g => g.Category);

            if (categoryIds != null && categoryIds.Any())
            {
                query = query.Where(g => categoryIds.Contains(g.CategoryId));
            }

            List<Good> goods = await query.ToListAsync();

            return goods;
        }

        [HttpPost("FindGoods")]
        public async Task<ActionResult<IEnumerable<Good>>> FindGoods([FromBody] string goodsTitle)
        {
            IQueryable<Good> query = context.Goods;
            if (goodsTitle != null)
            {
                query = query.Where(g => g.Title.Contains(goodsTitle));
            }

            List<Good> goods = await query.ToListAsync();

            return goods;
        }
    }

}

