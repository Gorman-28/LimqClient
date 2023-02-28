using LimqClient.Models;
using LimqClient.Settings;
using Microsoft.AspNetCore.Mvc;
using MyNamespace;
using System.Security.Cryptography;
using System.Text;

namespace LimqClient.Controllers
{
    public class LogInController : Controller
    {
        private readonly IClient client;

        public LogInController(IClient client)
        {
            this.client = client;
        }

        [HttpPost]
        public async Task<IActionResult> Check(UserForLogIn user)
        {
            if (ModelState.IsValid)
            {
                var md5 = MD5.Create();
                var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(user.Password));
                var hashPasword = Convert.ToBase64String(hash);
                var userToFind = await client.GetUserAsync(user.UserName, hashPasword);
                if (userToFind is null)
                {
                    ViewData["Theme"] = Request.Cookies.ContainsKey("blackTheme") ? SettingArray.blackTheme : SettingArray.whiteTheme;
                    ViewData["NoUser"] = "UserName or Password is incorrect";
                    return View("../Home/LogIn");
                }
                SettingArray.MyUser = await client.GetUserAsync(user.UserName, hashPasword);
                Response.Cookies.Append("UserName", $"{user.UserName}");
                Response.Cookies.Append("Password", $"{hashPasword}");
                return RedirectToAction("Chats", "Menu");
            }
            ViewData["NoUser"] = "";
            ViewData["Theme"] = Request.Cookies.ContainsKey("blackTheme") ? SettingArray.blackTheme : SettingArray.whiteTheme;
            return View("../Home/LogIn");
        }  

        
        
    }
}
