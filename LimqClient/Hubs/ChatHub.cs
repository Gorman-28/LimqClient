using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;
using System;
using LimqClient.Settings;

namespace LimqClient.Hubs
{
   
    public class ChatHub : Hub
    {

        public async Task JoinGroups(MyNamespace.GetAllChatsDto[] chats)
        {
            foreach(var chat in chats)
                await Groups.AddToGroupAsync(Context.ConnectionId, $"{SettingArray.MyUser.Id}+{chat.Id.ToString()}" );
        }

        public async Task SendMessage(MyNamespace.MessageChat message)
        {
            await Clients.All.SendAsync("SendMessage", message);
        }

        public Task SendMessageToGroup(MyNamespace.MessageChat message, Guid id)
        {
            return Clients.Group($"{id.ToString()}+{SettingArray.MyUser.Id}").SendAsync("SendMessage", message);
        }
    }
    
}
