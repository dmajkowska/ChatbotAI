namespace ChatbotAI.API.Domain.Interfaces.Services
{
    public interface IChatbotService
    {
        Task<List<string>> GenerateRandomAnswerAsync();
    }
}
