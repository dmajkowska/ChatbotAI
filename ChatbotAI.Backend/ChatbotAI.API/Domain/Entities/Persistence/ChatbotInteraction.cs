﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ChatbotAI.API.Domain.Entities.Persistence
{
    [Table("chatbot")]
    public class ChatbotInteraction
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("question")]
        public string QuestionContent { get; set; } = string.Empty;
        [Column("answer")]
        public string AnswerContent { get; set; } = string.Empty;
        [Column("rating")]
        public bool? Rating { get; set; } = null;
        [Column("created")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Column("interrupted")]
        public DateTime? InterrupteddAt { get; set; }
    }
}
