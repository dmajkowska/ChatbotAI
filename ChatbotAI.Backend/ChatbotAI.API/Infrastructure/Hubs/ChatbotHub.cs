using Microsoft.AspNetCore.SignalR;

namespace ChatbotAI.API.Infrastructure.Hubs
{
    public class ChatbotHub : Hub
    {
        public async Task SendAnswerPiece(int id, string section)
        {
            await Clients.All.SendAsync("ReceiveAnswerPiece", id, section);
        }
    }
}
