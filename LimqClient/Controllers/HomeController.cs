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
            if(Request.Cookies.ContainsKey("UserName"))
                return RedirectToAction("Enter", "Menu");
            return View(SettingArray.whiteTheme);
        }

        public IActionResult LogIn()
        {
            ViewData["Theme"] = SettingArray.whiteTheme;
            return View();
        }

        public IActionResult SignUp()
        {
            ViewData["Theme"] = SettingArray.whiteTheme;
            return View();
        }
    }
}