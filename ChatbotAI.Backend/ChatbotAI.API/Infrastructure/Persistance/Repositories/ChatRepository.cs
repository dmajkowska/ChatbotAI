using ChatbotAI.API.Domain.Entities.Persistence;
using ChatbotAI.API.Domain.Interfaces.Repositories;
using System;
//using Microsoft.EntityFrameworkCore;

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
    }
}
