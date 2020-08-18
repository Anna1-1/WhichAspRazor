using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLibrary.Data;
using DataLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RPDemo.Pages.Food
{
    public class MenuListModel : PageModel
    {
        private readonly IFoodData foodData; //food data private field

        public List<FoodModel> Food { get; set; } //list of food data public property - so can access this from view

        public MenuListModel(IFoodData foodData) //ctor
        {
            this.foodData = foodData;
        }

        public async Task OnGet()
        {
            Food = await foodData.GetFood();
        }
    }
}