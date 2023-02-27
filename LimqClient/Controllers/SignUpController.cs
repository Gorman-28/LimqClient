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
                    var file = "./wwwroot/img/logo.png";
                    using var stream = new MemoryStream(System.IO.File.ReadAllBytes(file).ToArray());

                    var avatar = new FormFile(stream, 0, stream.Length, "streamFile", file.Split(@"\").Last());

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

                    return RedirectToAction("Chats", "Menu");

                }
                else
                {
                    ViewData["Theme"] = SettingArray.theme;
                    ViewData["NoUnique"] = "This UserName is already exists";
                    return View("../Home/SignUp");
                }
            }
            ViewData["NoUnique"] = "";
            ViewData["Theme"] = SettingArray.theme;
            return View("../Home/SignUp");
        }
    }
}
