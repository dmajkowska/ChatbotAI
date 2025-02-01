using MediatR;

namespace ChatbotAI.API.Application.Command.SaveAnswer
{
    public class SaveAnswerCommand : IRequest
    {
        public int Id { get; set; }
        public string AnswerContent { get; set; } = string.Empty;
        public bool IsInterupted { get; set; }
    }
}
