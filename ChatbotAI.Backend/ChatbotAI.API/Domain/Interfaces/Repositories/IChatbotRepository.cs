using ChatbotAI.API.Domain.Entities.Persistence;

namespace ChatbotAI.API.Domain.Interfaces.Repositories
{
    public interface IChatbotRepository
    {
        Task AddAsync(ChatbotInteraction chat);
        Task UpdateRatingAsync(int id, bool? newRating);
        Task TruncateAnswerAsync(int id, int displayedCharactersCount);
    }
}
