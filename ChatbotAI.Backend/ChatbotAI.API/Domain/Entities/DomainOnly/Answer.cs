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
            if (properties.AnswerContent.Length < 3) throw new DomainException("Odpowiedź jest za krótka.");
            if (ContainsBadWords(properties.AnswerContent)) throw new DomainException("Odpowiedź zawiera niedozwolone słowa.");

            Id = properties.Id;
            AnswerContent = properties.AnswerContent;
            IsInterrupted = properties.IsInterrupted;
        }

        private bool ContainsBadWords(string question)
        {
            var badWords = new[] { "brzydkieslowo1", "brzydkieslowo2" };
            return badWords.Any(question.ToLower().Contains);
        }
    }

    public class AnswerProperties
    {
        public int Id { get; set; }
        public string AnswerContent { get; set; } = string.Empty;
        public bool IsInterrupted { get; set; }
    }
}
