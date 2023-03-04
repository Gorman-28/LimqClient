using LimqClient.Settings;
using Microsoft.AspNetCore.Mvc;
using MyNamespace;

namespace LimqClient.Controllers
{
    public class SquadController : Controller
    {
        private readonly IClient client;

        public SquadController(IClient client)
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
            var squad = await client.GetSquadsAsync(SettingArray.MyUser.Id);
            var messages = await client.GetMessagesSquadAsync(guidId);
            ViewData["squad"] = squad.FirstOrDefault(c => c.Id == guidId);
            ViewData["Theme"] = Request.Cookies.ContainsKey("blackTheme") ? SettingArray.blackTheme : SettingArray.whiteTheme;
            return View(messages);
        }

        public async Task<IActionResult> CreateMessage(string id, string message)
        {
            var newMessage = new MyNamespace.CreateMessageSquadRequest
            {
                SquadId = Guid.Parse(id),
                UserFromId = SettingArray.MyUser.Id,
                Message = message,
                MessageTime = DateTime.UtcNow,
                SystemMessage = false,
            };
            await client.CreateMessageSquadAsync(newMessage);
            return Ok();
        }
    }
}
