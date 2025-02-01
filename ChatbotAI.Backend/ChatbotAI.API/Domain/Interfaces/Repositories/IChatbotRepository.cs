using ChatbotAI.API.Domain.Entities.DomainOnly;
using ChatbotAI.API.Domain.Entities.Persistence;

namespace ChatbotAI.API.Domain.Interfaces.Repositories
{
    public interface IChatbotRepository
    {
        Task AddAsync(ChatbotInteraction chat);
        Task UpdateAnswerAsync(int id, string answer, bool isInterrupted);
        Task UpdateRatingAsync(int id, bool? newRating);
    }
}
