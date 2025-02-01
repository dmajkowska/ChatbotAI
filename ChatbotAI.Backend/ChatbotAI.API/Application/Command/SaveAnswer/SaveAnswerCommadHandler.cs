using ChatbotAI.API.Domain.Entities.DomainOnly;
using ChatbotAI.API.Domain.Interfaces.Repositories;
using MediatR;

namespace ChatbotAI.API.Application.Command.SaveAnswer
{
    public class SaveAnswerCommadHandler : IRequestHandler<SaveAnswerCommand>
    {
        private readonly IChatbotRepository _chatbotRepository;

        public SaveAnswerCommadHandler(IChatbotRepository chatRepository)
        {
            _chatbotRepository = chatRepository ?? throw new ArgumentNullException(nameof(chatRepository));
        }

        public async Task Handle(SaveAnswerCommand request, CancellationToken cancellationToken)
        {
            var answerParameters = new AnswerProperties()
            {
                AnswerContent = request.AnswerContent,
                Id = request.Id,
                IsInterrupted = request.IsInterupted
            };

            Answer answer = new Answer(answerParameters);

            await _chatbotRepository.UpdateAnswerAsync(answer.Id, answer.AnswerContent, answer.IsInterrupted);
        }
    }
}


