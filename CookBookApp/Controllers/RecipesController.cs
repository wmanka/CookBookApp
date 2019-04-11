using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CookBookApp.Controllers
{
    public class RecipesController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }
    }
}