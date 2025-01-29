using ChatbotAI.API.Contract.TruncateAnswer;
using MediatR;

namespace ChatbotAI.API.Application.Command.TruncateAnswer
{
    public class TruncateAnswerCommand : TruncateAnswerRequest, IRequest
    {
    }
}
