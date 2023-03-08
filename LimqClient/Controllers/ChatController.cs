using LimqClient.Settings;
using Microsoft.AspNetCore.Mvc;
using MyNamespace;
using System.Xml.Linq;


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
        public async Task<IActionResult> FindUser(string name)
        {
            var users = await client.GetUsersAsync(name);
            var chats = await client.GetChatsAsync(LimqClient.Settings.SettingArray.MyUser.Id);

            if (chats == null) 
                return PartialView(users);
            else
            {
                foreach (var item in chats)
                {
                    foreach(var user in users)
                    {
                        if(user.Id == item.Id)
                            users.Remove(user);
                    }
                }

                return PartialView(users);
            }
        }
        
        public async Task<IActionResult> Speak(Guid guidId)
        {
            var chat = await client.GetChatsAsync(SettingArray.MyUser.Id);
            var messages = await client.GetMessagesChatsAsync(SettingArray.MyUser.Id, guidId);
            ViewData["chat"] = chat.FirstOrDefault(c => c.Id == guidId);
            ViewData["Theme"] = Request.Cookies.ContainsKey("blackTheme") ? SettingArray.blackTheme : SettingArray.whiteTheme;
            return View(messages);
        }

        public async Task<IActionResult> GetChats()
        {
            var chats = await client.GetChatsAsync(LimqClient.Settings.SettingArray.MyUser.Id);

             return PartialView(chats);
            
        }

        public async Task<IActionResult> GetChatsWithName(string name)
        {
            var chats = await client.GetChatsAsync(LimqClient.Settings.SettingArray.MyUser.Id);

            return PartialView("GetChats", chats.OrderBy(c=> c.UserName.Contains(name)));

        }
        
        public async Task<IActionResult> CreateChat(string id)
        {
        var newChat = new MyNamespace.CreateChatRequest
        {
            FirstUser = SettingArray.MyUser.Id,
            SecondUser = Guid.Parse(id)
        };
        await client.CreateChatAsync(newChat);

        var newMessage = new MyNamespace.CreateMessageChatRequest
        {
            UserFromId = SettingArray.MyUser.Id,
            UserToId = Guid.Parse(id),
            Message = "Hi",
            MessageTime = DateTime.UtcNow
        };
        await client.CreateMessagesChatsAsync(newMessage);

        return RedirectToAction("Speak", new { guidId = Guid.Parse(id) });
        }

        public async Task<IActionResult> CreateMessage(string id, string message)
        {
            var newMessage = new MyNamespace.CreateMessageChatRequest
            {
                UserFromId = SettingArray.MyUser.Id,
                UserToId = Guid.Parse(id),
                Message = message,
                MessageTime = DateTime.UtcNow
            };
            await client.CreateMessagesChatsAsync(newMessage);
            return Ok();
        }
    }
}
