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

       
        public async Task<IActionResult> Check(User user)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(user.Password));
            var hashPasword = Convert.ToBase64String(hash);
            var userToFind = await client.GetUserAsync(user.UserName, hashPasword);
            if (userToFind is null)
            {
                ViewData["whiteTheme"] = SettingArray.whiteTheme;
                ViewData["NoUser"] = "No user with such name or password";
                return View("../Home/LogIn");
            }

            return View("../Menu/MainMenu");
        }  

        
        
    }
}
