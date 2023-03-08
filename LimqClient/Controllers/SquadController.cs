using LimqClient.Models;
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

        public async Task<IActionResult> GetSquads()
        {
            var squads = await client.GetSquadsAsync(SettingArray.MyUser.Id);

            return PartialView(squads);

        }

        public async Task<IActionResult> GetSquadsWithName(string name)
        {
            var squads = await client.GetSquadsAsync(SettingArray.MyUser.Id);

            return PartialView(squads.OrderBy(s => s.Name.Contains(name)));

        }

        public async Task<IActionResult> FindUser(string name)
        {
            var users = await client.GetUsersAsync(name);
            var usersWithoutMe = users.Where(u => u.Id != SettingArray.MyUser.Id);


            return PartialView(usersWithoutMe);
            

        }

        public async Task<IActionResult> CreateSquad(string[] usersArray, string name)
        {

            var file = "./wwwroot/img/logoL.png";
            using var stream = new MemoryStream(System.IO.File.ReadAllBytes(file).ToArray());

            var avatar = new FormFile(stream, 0, stream.Length, "streamFile", file.Split(@"\").Last());

            var id = await client.CreateSquadAsync(name, avatar, SettingArray.MyUser.Id);

            var me = new MyNamespace.CreateUserSquadRequest()
            {
                SquadId = id,
                UserId = SettingArray.MyUser.Id
            };
            await client.CreateUserSquadAsync(me);


            foreach (var user in usersArray)
            {
                var newUser = new MyNamespace.CreateUserSquadRequest()
                {
                    SquadId = id,
                    UserId = Guid.Parse(user)
                };
                await client.CreateUserSquadAsync(newUser);
            }

            var newMessage = new MyNamespace.CreateMessageSquadRequest
            {
                UserFromId = SettingArray.MyUser.Id,
                SquadId = id,
                Message = $"Group {name} was created",
                MessageTime = DateTime.UtcNow,
                SystemMessage = true
            };
            await client.CreateMessageSquadAsync(newMessage);

            return RedirectToAction("Speak", new { guidId = SettingArray.MyUser.Id });


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
