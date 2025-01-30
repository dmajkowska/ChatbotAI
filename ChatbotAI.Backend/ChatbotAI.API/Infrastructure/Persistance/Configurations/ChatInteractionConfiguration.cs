using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ChatbotAI.API.Domain.Entities.Persistence;

namespace ChatbotAI.API.Infrastructure.Persistance.Configurations
{
    public class ChatInteractionConfiguration : IEntityTypeConfiguration<ChatInteraction>
    {
        public void Configure(EntityTypeBuilder<ChatInteraction> builder)
        {
            builder.HasKey(q => q.Id);
            builder.Property(q => q.QuestionContent)
                .IsRequired()
                .HasMaxLength(500);
        }
    }
}