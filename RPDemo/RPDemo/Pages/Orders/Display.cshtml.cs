using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLibrary.Data;
using DataLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RPDemo.Models;

namespace RPDemo.Pages.Orders
{
    public class DisplayModel : PageModel
    {
        private readonly IOrderData orderData;
        private readonly IFoodData foodData;

        [BindProperty(SupportsGet = true)] //can pass in the  value of the property as part of the request url on a get and not just a post
        public int Id { get; set; } // to know which record to update info/name in this case for

        [BindProperty]
        public OrderUpdateModel UpdateModel { get; set; }

        public OrderModel Order { get; set; }
        public string ItemPurchased { get; set; }

        public DisplayModel(IOrderData orderData, IFoodData foodData)
        {
            this.orderData = orderData;
            this.foodData = foodData;
        }
        public async Task<IActionResult> OnGet()
        {
            Order = await orderData.GetOrderById(Id); // look up order

            if (Order != null) // if there is an order with that id
            {
                var food = await foodData.GetFood(); // get food

                //query - where food id matches the purchase id - if it exists return the item title - otherwise, if don't find it return null
                ItemPurchased = food.Where(x => x.Id == Order.FoodId).FirstOrDefault()?.Title; 
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid == false)
            {
                return Page();
            }

            await orderData.UpdateOrderName(UpdateModel.Id, UpdateModel.OrderName); //Data access

            return RedirectToPage("./Display", new { UpdateModel.Id });
        }
    }
}
