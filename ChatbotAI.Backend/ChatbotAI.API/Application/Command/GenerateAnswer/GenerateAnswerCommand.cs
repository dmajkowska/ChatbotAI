using MediatR;

namespace ChatbotAI.API.Application.Command.GenerateAnswer
{
    public class GenerateAnswerCommand : IRequest<List<string>>
    {
    }
}
