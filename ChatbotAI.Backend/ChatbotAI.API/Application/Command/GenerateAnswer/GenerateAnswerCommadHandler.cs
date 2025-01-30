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
        private readonly IChatRepository _chatRepository;
        private readonly IChatService _chatService;

        public GenerateAnswerCommadHandler(IChatRepository chatRepository, IChatService chatService)
        {
            _chatRepository = chatRepository ?? throw new ArgumentNullException(nameof(chatRepository));
            _chatService = chatService ?? throw new ArgumentNullException(nameof(chatService));
        }

        public async Task<GenerateAnswerResponse> Handle(GenerateAnswerCommand request, CancellationToken cancellationToken)
        {
            var question = new Question(request.Question);

            if (question == null)
            {
                throw new DomainException("Nie rozpoznano pytania");
            }

            var answer = await _chatService.GenerateRandomAnswerAsync();

            var chat = new ChatInteraction()
            {
                AnswerContent = string.Join("\n\n", answer),
                QuestionContent = question.Content,
                CreatedAt = DateTime.UtcNow,
            };

            await _chatRepository.AddAsync(chat);

            var response = new GenerateAnswerResponse()
            {
                Id = chat.Id,
                Question = question.Content,
                SectionList = answer
            };

            return response;
        }
    }
}



