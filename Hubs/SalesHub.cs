using Microsoft.AspNetCore.SignalR;

namespace ChartsExampleServer.Hubs
{
    public class SalesHub : Hub
    {
        public async Task SendMessage()
        {
            await Clients.All.SendAsync("receiveMessage","Merhaba");
        }
    }
}
