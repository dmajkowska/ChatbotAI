using ChatbotAI.API.Application.Command.GenerateAnswer;
using ChatbotAI.API.Contract.InteractWithChatbot;
using ChatbotAI.API.Domain.Entities.DomainOnly;
using ChatbotAI.API.Domain.Entities.Persistence;
using ChatbotAI.API.Domain.Exceptions;
using ChatbotAI.API.Domain.Interfaces.Repositories;
using ChatbotAI.API.Domain.Interfaces.Services;
using MediatR;

namespace ChatbotAI.API.Application.Command.AddChatbotInteraction
{
    public class AddChatbotInteractionCommadHandler : IRequestHandler<AddChatbotInteractionCommand, InteractWithChatbotResponse>
    {
        private readonly IChatbotRepository _chatbotRepository;
        private readonly IMediator _mediator;

        public AddChatbotInteractionCommadHandler(IChatbotRepository chatRepository, IMediator mediator)
        {
            _chatbotRepository = chatRepository ?? throw new ArgumentNullException(nameof(chatRepository));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<InteractWithChatbotResponse> Handle(AddChatbotInteractionCommand request, CancellationToken cancellationToken)
        {
            var question = new Question(request.Question);

            if (question == null)
            {
                throw new DomainException("Nie rozpoznano pytania");
            }

            var command = new GenerateAnswerCommand();
            var answer = await _mediator.Send(command); ;

            var chatbot = new ChatbotInteraction()
            {
                QuestionContent = question.Content,
                CreatedAt = DateTime.UtcNow,
            };

            await _chatbotRepository.AddAsync(chatbot);

            var response = new InteractWithChatbotResponse()
            {
                Id = chatbot.Id,
                Question = question.Content,
                SectionList = answer
            };

            return response;
        }
    }
}



