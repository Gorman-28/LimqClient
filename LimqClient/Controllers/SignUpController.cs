using LimqClient.Settings;
using Microsoft.AspNetCore.Mvc;
using MyNamespace;
using System.Security.Cryptography;
using System.Text;
using LimqClient.Models;
using User = LimqClient.Models.User;

namespace LimqClient.Controllers
{
    public class SignUpController : Controller
    {
        private readonly IClient client;

        public SignUpController(IClient client)
        {
            this.client = client;
        }

        [HttpPost]
        public async Task<IActionResult> Check(User user)
        {
            if (ModelState.IsValid)
            {
                var uniqUser = await client.CheckUserAsync(user.UserName);
                if (uniqUser is null)
                {
                    using var stream = System.IO.File.OpenRead("wwwroot/img/standartAvatar.svg");

                    var avatar = new FileParameter(stream, Path.GetFileName(stream.Name));

                    var md5 = MD5.Create();
                    var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(user.Password));
                    var hashPasword = Convert.ToBase64String(hash);
                    await client.CreateUserAsync(user.UserName, hashPasword, user.FirstName, user.LastName, avatar);
                    SettingArray.MyUser = await client.GetUserAsync(user.UserName, hashPasword);
                    await client.ChangeStatusAsync(new ChangeStatusRequest()
                    {
                        Id = SettingArray.MyUser.Id,
                        Status = true
                    });

                    ViewData["Theme"] = SettingArray.whiteTheme;
                    return View("../Menu/MainMenu");

                }
                else
                {
                    ViewData["Theme"] = SettingArray.whiteTheme;
                    ViewData["NoUnique"] = "This UserName is already exists";
                    return View("../Home/SignUp");
                }
            }
            ViewData["NoUnique"] = "";
            ViewData["Theme"] = SettingArray.whiteTheme;
            return View("../Home/SignUp");
        }
    }
}
