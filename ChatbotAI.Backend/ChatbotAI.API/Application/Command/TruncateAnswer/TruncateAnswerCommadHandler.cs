﻿using ChatbotAI.API.Domain.Entities.DomainOnly;
using ChatbotAI.API.Domain.Exceptions;
using ChatbotAI.API.Domain.Interfaces.Repositories;
using ChatbotAI.API.Domain.Interfaces.Services;
using MediatR;

namespace ChatbotAI.API.Application.Command.TruncateAnswer
{
    public class TruncateAnswerCommadHandler : IRequestHandler<TruncateAnswerCommand>
    {
        private readonly IChatbotRepository _chatbotRepository;

        public TruncateAnswerCommadHandler(IChatbotRepository chatRepository, IChatbotService chatService)
        {
            _chatbotRepository = chatRepository ?? throw new ArgumentNullException(nameof(chatRepository));
        }

        public async Task Handle(TruncateAnswerCommand request, CancellationToken cancellationToken)
        {
            var truncateParameters = new TruncateParameters(request.Id, request.DisplayedCharactersCount);

            if (truncateParameters == null)
            {
                throw new DomainException("Nieznane parametry dla przerwania wyświetlania odpowiedzi.");
            }

            await _chatbotRepository.TruncateAnswerAsync(truncateParameters.Id, truncateParameters.DisplayedCharactersCount);
        }
    }
}


