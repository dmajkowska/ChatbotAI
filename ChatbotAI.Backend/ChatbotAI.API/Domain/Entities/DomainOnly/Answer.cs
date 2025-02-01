using ChatbotAI.API.Domain.Exceptions;

namespace ChatbotAI.API.Domain.Entities.DomainOnly
{
    public class Answer
    {
        public int Id { get; private set; }
        public string AnswerContent { get; private set; }
        public bool IsInterrupted { get; private set; }

        public Answer(AnswerProperties properties)
        {
            if (properties.Id <= 0) throw new DomainException("Identyfikator nie może być ujemny ani równy zero.");

            Id = properties.Id;
            AnswerContent = properties.AnswerContent;
            IsInterrupted = properties.IsInterrupted;
        }
    }

    public class AnswerProperties
    {
        public int Id { get; set; }
        public string AnswerContent { get; set; } = string.Empty;
        public bool IsInterrupted { get; set; }
    }
}
