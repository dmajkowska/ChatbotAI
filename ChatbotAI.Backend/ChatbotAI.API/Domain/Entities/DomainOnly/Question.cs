using ChatbotAI.API.Domain.Exceptions;

namespace ChatbotAI.API.Domain.Entities.DomainOnly
{
    public class Question
    {
        public string Content { get; private set; }

        public Question(string question)
        {
            if (string.IsNullOrWhiteSpace(question)) throw new DomainException("Pytanie nie może być puste.");
            if (question.Length < 3) throw new DomainException("Pytanie jest za krótkie.");
            if (ContainsBadWords(question)) throw new DomainException("Pytanie zawiera niedozwolone słowa.");

            Content = question;
        }

        private bool ContainsBadWords(string question)
        {
            var badWords = new[] { "brzydkieslowo1", "brzydkieslowo2" };
            return badWords.Any(question.ToLower().Contains);
        }
    }
}
