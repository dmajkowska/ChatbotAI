using ChatbotAI.API.Domain.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;

namespace ChatbotAI.API.Hubs
{
    public class ChatbotHub : Hub
    {
        private readonly IChatbotService _chatbotService;

        public ChatbotHub(IChatbotService chatbotService)
        {
            _chatbotService = chatbotService ?? throw new ArgumentNullException(nameof(chatbotService));
        }

        public async Task StopAnswer(int id)
        {
            await _chatbotService.StopAnswer(id);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine($"Użytkownik rozłączony: {Context.ConnectionId}, powód: {exception?.Message}");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
