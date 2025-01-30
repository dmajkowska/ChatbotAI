using ChatbotAI.API.Contract.GenerateAnswer;
using ChatbotAI.API.Domain.Entities.DomainOnly;
using ChatbotAI.API.Domain.Entities.Persistence;
using ChatbotAI.API.Domain.Exceptions;
using ChatbotAI.API.Domain.Interfaces.Repositories;
using ChatbotAI.API.Domain.Interfaces.Services;
using MediatR;

namespace ChatbotAI.API.Application.Command.GenerateAnswer
{
    public class GenerateAnswerCommadHandler : IRequestHandler<GenerateAnswerCommand, GenerateAnswerResponse>
    {
        private readonly IChatbotRepository _chatbotRepository;
        private readonly IChatbotService _chatbotService;

        public GenerateAnswerCommadHandler(IChatbotRepository chatRepository, IChatbotService chatService)
        {
            _chatbotRepository = chatRepository ?? throw new ArgumentNullException(nameof(chatRepository));
            _chatbotService = chatService ?? throw new ArgumentNullException(nameof(chatService));
        }

        public async Task<GenerateAnswerResponse> Handle(GenerateAnswerCommand request, CancellationToken cancellationToken)
        {
            var question = new Question(request.Question);

            if (question == null)
            {
                throw new DomainException("Nie rozpoznano pytania");
            }

            var answer = await _chatbotService.GenerateRandomAnswerAsync();

            var chatbot = new ChatbotInteraction()
            {
                AnswerContent = string.Join("\n\n", answer),
                QuestionContent = question.Content,
                CreatedAt = DateTime.UtcNow,
            };

            await _chatbotRepository.AddAsync(chatbot);

            var response = new GenerateAnswerResponse()
            {
                Id = chatbot.Id,
                Question = question.Content,
                SectionList = answer
            };

            return response;
        }
    }
}



