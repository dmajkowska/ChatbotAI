using ChatbotAI.API.Domain.Entities.DomainOnly;
using ChatbotAI.API.Domain.Exceptions;
using ChatbotAI.API.Domain.Interfaces.Repositories;
using ChatbotAI.API.Domain.Interfaces.Services;
using MediatR;

namespace ChatbotAI.API.Application.Command.RateAnswer
{
    public class RateAnswerCommadHandler : IRequestHandler<RateAnswerCommand>
    {
        private readonly IChatbotRepository _chatbotRepository;

        public RateAnswerCommadHandler(IChatbotRepository chatRepository, IChatbotService chatService)
        {
            _chatbotRepository = chatRepository ?? throw new ArgumentNullException(nameof(chatRepository));
        }

        public async Task Handle(RateAnswerCommand request, CancellationToken cancellationToken)
        {
            var rate = new Rate(request.Id, request.Rating);

            if (rate == null)
            {
                throw new DomainException("Nie rozpoznano odpowiedzi");
            }

            await _chatbotRepository.UpdateRatingAsync(rate.Id, rate.Rating);
        }
    }
}
