using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var goods = context.Goods.ToList();
            return goods;
        }

        [HttpPost("CreateGoods")]
        public async Task<ActionResult> CreateGoods(Good good)
        {
            try
            {
                good.Category = null;
                context.Goods.Add(good);
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


        //запросы к заказу(order)
        [HttpGet("GetOrder")] 
        public async Task<List<Order>> GetOrder()
        {
            await Task.Delay(10);
            var order = context.Orders.ToList();
            return order;
        }


    }
}
