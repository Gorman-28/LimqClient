using LimqClient.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LimqClient.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}