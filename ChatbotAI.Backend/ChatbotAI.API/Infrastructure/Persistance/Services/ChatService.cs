using ChatbotAI.API.Domain.Interfaces.Services;
using LoremNET;

namespace ChatbotAI.API.Infrastructure.Persistance.Services
{
    public class ChatService: IChatService
    {
        public async Task<List<string>> GenerateRandomAnswerAsync()
        {
            Random random = new Random();
            TextSize textSize = (TextSize)random.Next(1, 3);

            var sectionList = new List<string>();

            switch (textSize)
            {
                case TextSize.Short:
                    sectionList = new List<string>()
                    {
                        Lorem.Paragraph(wordCountMin: 1, wordCountMax: 10, sentenceCountMin: 1, sentenceCountMax: 2)
                    }; break;
                case TextSize.Medium:
                    sectionList = new List<string>()
                    {
                        Lorem.Paragraph(wordCountMin: 1, wordCountMax: 10, sentenceCountMin: 3, sentenceCountMax: 5)
                    }; break;
                default:
                    sectionList = Lorem.Paragraphs(wordCountMin: 1, wordCountMax: 10, sentenceCountMin: 3, sentenceCountMax: 5, paragraphCountMin: 3, paragraphCountMax: 10).ToList();
                    break;
            }

            return sectionList;
        }
        private enum TextSize
        {
            Short,
            Medium,
            Long
        }
    }
}
