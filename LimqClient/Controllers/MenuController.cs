using LimqClient.Settings;
using Microsoft.AspNetCore.Mvc;
using MyNamespace;

namespace LimqClient.Controllers
{
    public class MenuController : Controller
    {
        private readonly IClient client;

        public MenuController(IClient client)
        {
            this.client = client;
        }
        public async Task<IActionResult> Enter()
        {
            var userName = Request.Cookies["UserName"];
            var password = Request.Cookies["Password"];
            SettingArray.MyUser = await client.GetUserAsync(userName, password);
            return RedirectToAction("Chats", "Menu");
        }
        

        public async Task<IActionResult> Chats()
        {
            var chats = await client.GetChatsAsync(SettingArray.MyUser.Id);
            ViewData["Theme"] = Request.Cookies.ContainsKey("blackTheme") ? SettingArray.blackTheme : SettingArray.whiteTheme;
            return View(chats);
        }

        public async Task<IActionResult> Squads()
        {
            var squads = await client.GetSquadsAsync(SettingArray.MyUser.Id);
            ViewData["Theme"] = Request.Cookies.ContainsKey("blackTheme") ? SettingArray.blackTheme : SettingArray.whiteTheme;
            return View(squads);
        }
    }
}
