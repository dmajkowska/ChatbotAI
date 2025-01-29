using ChatbotAI.API.Domain.Exceptions;

namespace ChatbotAI.API.Domain.Entities.DomainOnly
{
    public class Rate
    {
        public int Id { get; private set; }
        public bool? Rating { get; private set; }

        public Rate(int id, bool? rate)
        {
            if (id <= 0) throw new DomainException("Identyfikator nie może być ujemny ani równy zero.");
            Id = id;
            Rating = rate;
        }
    }
}
