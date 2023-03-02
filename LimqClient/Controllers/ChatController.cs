using LimqClient.Settings;
using Microsoft.AspNetCore.Mvc;
using MyNamespace;
using System;

namespace LimqClient.Controllers
{
    public class ChatController : Controller
    {
        private readonly IClient client;

        public ChatController(IClient client)
        {
            this.client = client;
        }
        public async Task<IActionResult> Main(string id)
        {
            var newId = Guid.Parse(id);
            return RedirectToAction("Speak", new { guidId = newId });
        }

        public async Task<IActionResult> Speak(Guid guidId)
        {
            var chat = await client.GetChatsAsync(SettingArray.MyUser.Id);
            var messages = await client.GetMessagesChatsAsync(SettingArray.MyUser.Id, guidId);
            ViewData["chat"] = chat.FirstOrDefault(c => c.Id == guidId);
            ViewData["Theme"] = Request.Cookies.ContainsKey("blackTheme") ? SettingArray.blackTheme : SettingArray.whiteTheme;
            return View(messages);
        }
    }
}
