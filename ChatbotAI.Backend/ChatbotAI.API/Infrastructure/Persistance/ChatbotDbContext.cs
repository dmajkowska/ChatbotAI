using ChatbotAI.API.Domain.Entities.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ChatbotAI.API.Infrastructure.Persistance
{
    public class ChatbotDbContext : DbContext
    {
        public ChatbotDbContext(DbContextOptions<ChatbotDbContext> options) : base(options) { }

        public DbSet<ChatbotInteraction> ChatbotInteractions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
