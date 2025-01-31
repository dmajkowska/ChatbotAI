using ChatbotAI.API.Domain.Interfaces.Services;
using LoremNET;
using System.Collections.Concurrent;

namespace ChatbotAI.API.Application.Services
{
    public class ChatbotService : IChatbotService
    {
        private readonly ConcurrentDictionary<int, bool> _stoppedResponses = new();

        public ChatbotService()
        {
            
        }

        public async Task StopAnswer(int id)
        {
            _stoppedResponses[id] = true;
        }

        public bool IsAnswerStopped(int id)
        {
            return _stoppedResponses.TryGetValue(id, out bool isStopped) && isStopped;
        }

        public void RemoveStoppedAnswer(int id)
        {
            _stoppedResponses.TryRemove(id, out _);
        }

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
