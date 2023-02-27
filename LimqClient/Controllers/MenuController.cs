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
        public async Task<IActionResult> Chats()
        {
            var chats = await client.GetChatsAsync(SettingArray.MyUser.Id);
            ViewData["Theme"] = SettingArray.theme;
            return View(chats);
        }
    }
}
