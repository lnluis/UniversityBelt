using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace UniversityBelt.Server.Hubs
{
    [HubName("universityChatHub")]
    public class ChatHub : Hub
    {
        public void SendMessage(string userName, string message)
        {
            var value = string.Format("{0} : {1}", userName, message);
            Clients.All.ReceivedMessage(value);
        }
    }
}