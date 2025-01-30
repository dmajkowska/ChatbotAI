using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ChatbotAI.API.Domain.Entities.Persistence;

namespace ChatbotAI.API.Infrastructure.Persistance.Configurations
{
    public class ChatbotInteractionConfiguration : IEntityTypeConfiguration<ChatbotInteraction>
    {
        public void Configure(EntityTypeBuilder<ChatbotInteraction> builder)
        {
            builder.HasKey(q => q.Id);
            builder.Property(q => q.QuestionContent)
                .IsRequired()
                .HasMaxLength(500);
        }
    }
}