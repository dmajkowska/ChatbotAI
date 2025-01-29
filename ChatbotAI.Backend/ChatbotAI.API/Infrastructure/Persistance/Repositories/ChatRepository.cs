using ChatbotAI.API.Domain.Entities.Persistence;
using ChatbotAI.API.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChatbotAI.API.Infrastructure.Persistance.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly ApplicationDbContext _context;

        public ChatRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ChatInteraction chat)
        {
            _context.ChatInteractions.Add(chat);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRatingAsync(int id, bool? newRating)
        {
            var chat = new ChatInteraction { Id = id, Rating = newRating };

            _context.Attach(chat);
            _context.Entry(chat).Property(x => x.Rating).IsModified = true;

            await _context.SaveChangesAsync();
        }

        public async Task TruncateAnswerAsync(int id, int displayedCharactersCount)
        {
            var chat = new ChatInteraction { Id = id, InterrupteddAt = DateTime.UtcNow };

            _context.Attach(chat);
            _context.Entry(chat).Property(x => x.Rating).IsModified = true;

            await _context.SaveChangesAsync();

            await _context.ChatInteractions
                .Where(entry => entry.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(entry => entry.AnswerContent, entry => entry.AnswerContent.Substring(0, displayedCharactersCount))
                    .SetProperty(entry => entry.InterrupteddAt, DateTime.UtcNow)
                );
        }
    }
}
