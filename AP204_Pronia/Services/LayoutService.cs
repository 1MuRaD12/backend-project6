using AP204_Pronia.DAL;
using AP204_Pronia.Models;
using AP204_Pronia.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AP204_Pronia.Services
{
    public class LayoutService
    {
        private readonly AppDbContext context;
        private readonly IHttpContextAccessor httpContextAccessor;

        public LayoutService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<Setting>> getData()
        {
            List<Setting> setting = await context.settings.ToListAsync();

            return setting;
        }

        public BasketVM getbasket()
        {
            string basket = httpContextAccessor.HttpContext.Request.Cookies["basket"];
            if (!string.IsNullOrEmpty(basket))
            {
                BasketVM basketVM = JsonConvert.DeserializeObject<BasketVM>(basket);

                return basketVM;
            }
            else
            {
                return null;
            }
        }
    }
}
