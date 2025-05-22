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

        [HttpPost("AddToBasket")]
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
                await context.SaveChangesAsync();
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
        public async Task<ActionResult> CreateReviewByGood(Review review)
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized("error");
                }
                context.Reviews.Add(review); //типа добавили и обновили товар - теперь у него есть отзыв
                await context.SaveChangesAsync();
                return Ok("Успешно");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<User>> GetUserData()
        {
            var id = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return Ok(context.Users.Find(id));
        }

        [HttpGet("GetOrderBasketCount/{userId}")] //получить кол-во товаров в  корзине
        public async Task<int?> GetCount(int userId)
        {
            await Task.Delay(10);
            var order = await context.Orders.FirstOrDefaultAsync(s => s.UserId == userId && s.Status == "корзина");
            var cr = context.OrderGoodsCrosses.Where(s => s.OrderId == order.Id);
            int? count = 0;
            foreach (var g in cr)
            {
                count += g.Quantity;
            }
            return count;
        }

        [HttpGet("GetGoodByOrder/{userId}")] //получить товары в корзине
        public async Task<List<OrderGoodsCross>> GetGoodByOrder(int userId)
        {
            await Task.Delay(10);
            var order = await context.Orders.FirstOrDefaultAsync(s => s.UserId == userId && s.Status == "корзина");
            var cr = context.OrderGoodsCrosses.Where(s => s.OrderId == order.Id).Include(s => s.Goods).ToList();

            return cr;
        }
  
        [HttpPut("StatusOrderFromActive/{userId}")]
        public async Task<IActionResult> StatusOrderFromActive(int userId)
        {
            // Поиск заказа по ID
            var order = await context.Orders.FirstOrDefaultAsync(s => s.UserId == userId);
            var cr = await context.OrderGoodsCrosses.FirstOrDefaultAsync(s => s.OrderId == order.Id);
            if (cr == null)
            {
                return NotFound("Заказ не найден");
            }

            // Проверка, что текущий статус - "cart"
            if (cr.Order.Status == "корзина")
            {
                cr.Order.Status = "активные";

                // Сохраняем изменения
                await context.SaveChangesAsync();
            }
            return Ok(new { message = "Заказ активирован", order });
        }

        [HttpGet("GetOrderDontActive")]  //получить выполненные заказы(в админку надо перенести)
        public async Task<List<OrderGoodsCross>> GetOrderDontActive()
        {
            await Task.Delay(10);
            var order = await context.Orders.FirstOrDefaultAsync(s => s.Status == "выполненные");
            var cr = context.OrderGoodsCrosses.Where(s => s.OrderId == order.Id).Include(s => s.Goods.Category).ToList();
            return cr;
        }

        [HttpGet("GetOrderActive")] //получить активные заказы(в админку надо перенести)
        public async Task<List<OrderGoodsCross>> GetOrderActive()
        {
            await Task.Delay(10);
            var order = await context.Orders.FirstOrDefaultAsync(s => s.Status == "активные");
            var cr = context.OrderGoodsCrosses.Where(s => s.OrderId == order.Id).Include(s => s.Goods.Category).ToList();

            return cr;
        }

        [HttpPut("SaveChangedByUserWin")]
        public async Task<ActionResult> SaveChangedByUserWin(User user)
        {
            try
            {
                context.Users.Update(user);
                await context.SaveChangesAsync();
                return Ok("Успешно");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
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

