using ChatbotAI.API.Contract.GenerateAnswer;
using MediatR;

namespace ChatbotAI.API.Application.Command.GenerateAnswer
{
    public class GenerateAnswerCommand : GenerateAnswerRequest, IRequest<GenerateAnswerResponse>
    {
    }
}
