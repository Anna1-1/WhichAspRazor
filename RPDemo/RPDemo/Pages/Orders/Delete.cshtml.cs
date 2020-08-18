using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLibrary.Data;
using DataLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RPDemo.Pages.Orders
{
    public class DeleteModel : PageModel
    {
        private readonly IOrderData orderData;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; } // to know which record to delete

        public OrderModel Order { get; set; }

        public DeleteModel(IOrderData orderData)
        {
            this.orderData = orderData;
        }

        public async Task OnGet()
        {
            Order = await orderData.GetOrderById(Id);
        }

        public async Task<IActionResult> OnPost()
        {
            // calling DeleteOrder method from AspNetCoreCommon's OrderData class (the data access layer?) 
            // which calls the 'dbo.spOrders_Delete' stored procedure which finds and deletes the record from the connected db
            await orderData.DeleteOrder(Id);

            return RedirectToPage("./Create"); //return value of Task<IActionResult> means that a task e.g. redirect to a page, is carried out on the return
        }
    }
}