using AP204_Pronia.DAL;
using AP204_Pronia.Models;
using AP204_Pronia.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AP204_Pronia.Controllers
{
    public class PlantController : Controller
    {
        private readonly AppDbContext context;

        public PlantController(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<IActionResult> Addbasket(int id)
        {
            Plant plant = await context.Plants.FirstOrDefaultAsync(s => s.Id == id);
            if (plant == null) return NotFound();
            string item = HttpContext.Request.Cookies["basket"];
            BasketVM basket;
            string itemstr;
            if (string.IsNullOrEmpty(item))
            {
                basket = new BasketVM();
                BasketItemVM basketItemVM = new BasketItemVM
                {
                    plant = plant,
                    Count = 1
                };
                basket.BasketItemVMs.Add(basketItemVM);
                itemstr = JsonConvert.SerializeObject(basketItemVM);

            }
            else
            {
                basket = JsonConvert.DeserializeObject<BasketVM>(item);

                BasketItemVM itemVM = basket.BasketItemVMs.FirstOrDefault(i => i.plant.Id == id);
                if (itemVM == null)
                {
                    BasketItemVM itemVM1 = new BasketItemVM
                    {
                        plant = plant,
                        Count = 1
                    };
                    basket.BasketItemVMs.Add(itemVM1);
                }
                else
                {
                    itemVM.Count++;
                }
                decimal total = default;
                foreach (BasketItemVM items in basket.BasketItemVMs)
                {
                    total += items.plant.Price * items.Count;
                };
                basket.ToyalPrice = total;
                basket.Count = basket.BasketItemVMs.Count;
                itemstr = JsonConvert.SerializeObject(basket);

            }
            HttpContext.Response.Cookies.Append("basket", itemstr);
            return RedirectToAction("Index", "Home");
        }
        public ContentResult Basket()
        {
            return Content(HttpContext.Request.Cookies["basket"]);

        }

    }
}
