using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.SymbolStore;
using System.Security.Claims;

namespace OnlineShopOA1135API.Controllers
{
    [Authorize]
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

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized("Не удалось получить UserId."); // Или другое сообщение об ошибке
            }

            Order activeOrder = await context.Orders.FirstOrDefaultAsync(o => o.UserId == userId); // Предполагаем, что у Order есть поле IsCompleted

            // 3. Если активной корзины нет, создаем новую.
            if (activeOrder == null)
            {
                activeOrder = new Order
                {
                    UserId = userId,
                    DateCreated = DateTime.UtcNow,
                   // IsCompleted = false // Устанавливаем флаг, что корзина активна
                };
                context.Orders.Add(activeOrder);
                await context.SaveChangesAsync();
            }

            // 4. Проверяем, есть ли уже этот товар в корзине. (Важно, если вы хотите обновлять количество)
            OrderGoodsCross existingItem = await context.OrderGoodsCrosses
                .FirstOrDefaultAsync(og => og.OrderId == activeOrder.Id && og.GoodsId == request.GoodId);

            if (existingItem != null)
            {
                // Если товар уже есть, можно обновить количество (предполагаем, что у OrderGoodsCross есть поле Quantity)
                //existingItem.Quantity += request.Quantity; //Добавлена логика для изменения количества
                //context.Update(existingItem);
            }
            else
            {
                // Если товара нет, добавляем новую запись.
                var orderGoodsCross = new OrderGoodsCross
                {
                    OrderId = activeOrder.Id,
                    GoodsId = request.GoodId,
                    //Quantity = request.Quantity // Добавлена инициализация Quantity
                };
                context.OrderGoodsCrosses.Add(orderGoodsCross);
            }

            await context.SaveChangesAsync();

            return Ok("Товар добавлен в корзину.");
        }

        /*  // 2. Находим или создаем текущий заказ (корзину) для пользователя.
          Order order = await context.Orders.FirstOrDefaultAsync(o => o.UserId == userId);
          // && o.IsCompleted == false  добавьте проверку на незавершенный заказ

          if (order == null)
          {
              order = new Order
              {
                  UserId = userId,
                  DateCreated = DateTime.UtcNow
              };
              context.Orders.Add(order);
              await context.SaveChangesAsync();
          }
          // 3. Проверяем, есть ли уже этот товар в корзине
          OrderGoodsCross existingItem = await context.OrderGoodsCrosses
                .FirstOrDefaultAsync(og => og.OrderId == order.Id && og.GoodsId == request.GoodId);

          // 4. Если товар уже есть в корзине, можно обновить количество (если это требуется).
          //    В данном примере мы просто добавляем еще одну запись.
          if (existingItem != null)
          {
              //Если вам нужно хранить количество, вам нужно добавить свойство Quantity в OrderGoodsCross и обновить его здесь.
              //
              //existingItem.Quantity += request.Quantity;
              //context.Update(existingItem);
          }

          // 5. Добавляем товар в корзину.
          var orderGoodsCross = new OrderGoodsCross
          {
              OrderId = order.Id,
              GoodsId = request.GoodId
          };

          context.OrderGoodsCrosses.Add(orderGoodsCross);
          await context.SaveChangesAsync();

          return Ok("Товар добавлен в корзину.");*/
    

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

