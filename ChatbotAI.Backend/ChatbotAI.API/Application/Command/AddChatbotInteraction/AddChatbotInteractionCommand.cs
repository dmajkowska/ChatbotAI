using ChatbotAI.API.Contract.InteractWithChatbot;
using MediatR;

namespace ChatbotAI.API.Application.Command.AddChatbotInteraction
{
    public class AddChatbotInteractionCommand : InteractWithChatbotRequest, IRequest<InteractWithChatbotResponse>
    {
    }
}
