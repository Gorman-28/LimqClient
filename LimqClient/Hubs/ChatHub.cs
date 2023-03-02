using Microsoft.AspNetCore.SignalR;
using LimqClient.Settings;
using MyNamespace;

namespace LimqClient.Hubs
{
   
    public class ChatHub : Hub
    {

        public async Task JoinGroup(string chat)
        {
            
            await Groups.AddToGroupAsync(Context.ConnectionId, $"{SettingArray.MyUser.Id}{chat}" );

        }

        public async Task SendMessage(string id, string message, string time)
        {
            await Clients.All.SendAsync("SendMessage", $"{id}", $"{message}", $"{time}");
        }

        public Task SendMessageToGroup(string id, string message, string time)
        {
            return Clients.Group($"{id}{SettingArray.MyUser.Id}").SendAsync("SendMessage", id, message, time);
        }
    }
    
}
