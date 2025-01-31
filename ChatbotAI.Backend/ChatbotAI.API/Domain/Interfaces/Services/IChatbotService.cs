namespace ChatbotAI.API.Domain.Interfaces.Services
{
    public interface IChatbotService
    {
        Task<List<string>> GenerateRandomAnswerAsync();
        Task StopAnswer(int id);
        bool IsAnswerStopped(int id);
        void RemoveStoppedAnswer(int id);
    }
}
