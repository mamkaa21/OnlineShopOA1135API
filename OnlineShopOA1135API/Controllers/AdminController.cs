using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace OnlineShopOA1135API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        readonly OnlineShopOa1135Context context;
        public AdminController(OnlineShopOa1135Context context)
        {
            this.context = context;
        }
        //запросы с товарами
        [HttpGet("GetGoods")]
        public async Task<List<Good>> GetGoods()
        {
            await Task.Delay(10);
            var goods = context.Goods.Include(s => s.Category).ToList();
            return goods;
        }

        [HttpPost("CreateGoods")]
        public async Task<ActionResult> CreateGoods(GoodModel good)
        {
            try
            {
                var goodNew = new Good { Title = good.Title, 
                    CategoryId = good.CategoryId, Price = good.Price,
                    Amount = good.Amount, Description =  good.Description, Image = good.Image, Review = good.Review, Rating = good.Rating };
                context.Goods.Add(goodNew);
                await context.SaveChangesAsync();
                return Ok("Успешно");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("EditGoods")]
        public async Task<ActionResult> EditGoods(Good good)
        {
            try
            {
                context.Goods.Update(good);
                await context.SaveChangesAsync();
                return Ok("Успешно");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("DeleteGoods")]
        public async Task<ActionResult> DeleteGoods(Good good)
        {
            try
            {
                context.Goods.Remove(good);
                await context.SaveChangesAsync();
                return Ok("Успешно");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        //запросы с категориями
        [HttpGet("GetCategories")]
        public async Task<List<Category>> GetCategories()
        {
            await Task.Delay(10);
            var categories = context.Categories.ToList();
            return categories;
        }

        [HttpPost("CreateCategories")]
        public async Task<ActionResult> CreateCategories(Category category)
        {
            try
            {
                context.Categories.Add(category);
                await context.SaveChangesAsync();
                return Ok("Успешно");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }


        }

        [HttpPut("EditCategpries")]
        public async Task<ActionResult> EditCategories(Category category)
        {
            try
            {
                context.Categories.Update(category);
                await context.SaveChangesAsync();
                return Ok("Успешно");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("DeleteCategories")]
        public async Task<ActionResult> DeleteCategories(Category category)
        {
            try
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
                return Ok("Успещно");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        //запросы с юзерами
        [HttpPost("CreateUsers")]
        public async Task<ActionResult> CreateUsers(User user)
        {
            try
            {
               
                context.Users.Add(user);
                await context.SaveChangesAsync();
                return Ok("Успешно");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("EditUsers")]
        public async Task<ActionResult> EditUsers(User user)
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

        [HttpDelete("DeleteUsers")]
        public async Task<ActionResult> DeleteUsers(User user)
        {
            try
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
                return Ok("Успешно");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        //запросы к заказу(order)
        [HttpGet("GetOrder")] 
        public async Task<List<Order>> GetOrder()
        {
            await Task.Delay(10);
            var order = context.Orders.ToList();
            return order;
        }

        [HttpPut("UpdateStatusOrder")] //скорее всего будет чекбок = если галка есть, то заказ помечается как выполненым и "удаляется" и переносится в графу выполненных заказов/история
        public async Task<ActionResult> UpdateStatusOrder(Order order)
        {
            context.Orders.Update(order);
            await context.SaveChangesAsync();
            return Ok();
        }


    }
}
