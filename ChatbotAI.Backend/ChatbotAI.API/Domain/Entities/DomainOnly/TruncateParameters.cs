using ChatbotAI.API.Domain.Exceptions;

namespace ChatbotAI.API.Domain.Entities.DomainOnly
{
    public class TruncateParameters
    {
        public int Id { get; private set; }
        public int DisplayedCharactersCount { get; private set; }

        public TruncateParameters(int id, int displayedCharactersCount)
        {
            if (id <= 0) throw new DomainException("Identyfikator nie może być ujemny ani równy zero.");
            if (displayedCharactersCount < 0) throw new DomainException("Liczba znaków nie może być ujemna.");
            Id = id;
            DisplayedCharactersCount = displayedCharactersCount;
        }
    }
}
