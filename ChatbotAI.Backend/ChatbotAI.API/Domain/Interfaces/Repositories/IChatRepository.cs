using ChatbotAI.API.Domain.Entities.Persistence;

namespace ChatbotAI.API.Domain.Interfaces.Repositories
{
    public interface IChatRepository
    {
        Task AddAsync(ChatInteraction chat);
        Task UpdateRatingAsync(int id, bool? newRating);
    }
}
