using ChatbotAI.API.Domain.Entities.DomainOnly;
using ChatbotAI.API.Domain.Entities.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ChatbotAI.API.Infrastructure.Persistance
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ChatInteraction> ChatInteractions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
