using LimqClient.Models;
using LimqClient.Settings;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LimqClient.Controllers
{
    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {
            return View(SettingArray.whiteTheme);
        }

        public IActionResult LogIn()
        {
            ViewData["whiteTheme"] = SettingArray.whiteTheme;
            return View();
        }
    }
}