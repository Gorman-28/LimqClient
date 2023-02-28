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
            return View(Request.Cookies.ContainsKey("blackTheme") ? SettingArray.blackTheme : SettingArray.whiteTheme);
        }

        public IActionResult LogIn()
        {
            ViewData["Theme"] = Request.Cookies.ContainsKey("blackTheme") ? SettingArray.blackTheme : SettingArray.whiteTheme;
            return View();
        }

        public IActionResult SignUp()
        {
            ViewData["Theme"] = Request.Cookies.ContainsKey("blackTheme") ? SettingArray.blackTheme : SettingArray.whiteTheme;
            return View();
        }
    }
}