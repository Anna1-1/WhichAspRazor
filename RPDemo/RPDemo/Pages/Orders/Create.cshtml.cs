using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLibrary.Data;
using DataLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RPDemo.Pages.Orders
{
    public class CreateModel : PageModel
    {
        private readonly IFoodData foodData;
        private readonly IOrderData orderData;

        public List<SelectListItem> FoodItems { get; set; }

        [BindProperty] // so that writing to order is allowed - setting this model on submit
        public OrderModel Order { get; set; }

        public CreateModel(IFoodData foodData, IOrderData orderData) // allows us to talk to the database through this clas and through each object derived from it
        {
            this.foodData = foodData;
            this.orderData = orderData;
        }
        public async Task OnGet()
        {
            var food = await foodData.GetFood();

            FoodItems = new List<SelectListItem>(); // newing up to ensure the list is empty at each instance

            //dropdown list functionality
            // filling dropdown - showing text (item title) but working with/storing Id (value)
            food.ForEach(x => 
            {
                FoodItems.Add(new SelectListItem { Value = x.Id.ToString(), Text = x.Title });
            });
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid == false)
            {
                return Page();
            }

            var food = await foodData.GetFood();

            Order.Total = Order.Quantity * food.Where(x => x.Id == Order.FoodId).First().Price;

            int id = await orderData.CreateOrder(Order);

            return RedirectToPage("./Display", new { Id = id });
        }
    }
}