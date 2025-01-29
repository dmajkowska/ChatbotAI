using ChatbotAI.API.Contract.RateAnswer;
using MediatR;

namespace ChatbotAI.API.Application.Command.RateAnswer
{
    public class RateAnswerCommand : RateAnswerRequest, IRequest
    {
    }
}
