using ChatbotAI.API.Domain.Entities.Persistence;
using ChatbotAI.API.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChatbotAI.API.Infrastructure.Persistance.Repositories
{
    public class ChatbotRepository : IChatbotRepository
    {
        private readonly ChatbotDbContext _context;

        public ChatbotRepository(ChatbotDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ChatbotInteraction chat)
        {
            _context.ChatbotInteractions.Add(chat);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAnswerAsync(int id, string answer, bool isInterrupted)
        {
            var chat = await _context.ChatbotInteractions.FindAsync(id);

            if (chat == null)
            {
                throw new KeyNotFoundException($"Nie odnaleziono rekordu o id {id}.");
            }

            chat.AnswerContent = answer;
            chat.InterrupteddAt = isInterrupted ? DateTime.UtcNow : null;

            _context.Update(chat);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateRatingAsync(int id, bool? newRating)
        {
            var chat = await _context.ChatbotInteractions.FindAsync(id);

            if (chat == null)
            {
                throw new KeyNotFoundException($"Nie odnaleziono rekordu o id {id}.");
            }

            chat.Rating = newRating;

            _context.Update(chat);

            await _context.SaveChangesAsync();
        }
    }
}
