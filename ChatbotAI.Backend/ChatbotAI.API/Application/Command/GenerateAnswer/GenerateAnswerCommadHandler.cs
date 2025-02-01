using ChatbotAI.API.Domain.Interfaces.Services;
using MediatR;

namespace ChatbotAI.API.Application.Command.GenerateAnswer
{
    public class GenerateAnswerCommadHandler : IRequestHandler<GenerateAnswerCommand, List<string>>
    {
        private readonly IChatbotService _chatbotService;

        public GenerateAnswerCommadHandler(IChatbotService chatService)
        {
            _chatbotService = chatService ?? throw new ArgumentNullException(nameof(chatService));
        }

        public async Task<List<string>> Handle(GenerateAnswerCommand request, CancellationToken cancellationToken)
        {
            var answer = await _chatbotService.GenerateRandomAnswerAsync();

            return answer;
        }
    }
}



