namespace ChatbotAI.API.Domain.Interfaces.Services
{
    public interface IChatService
    {
        Task<List<string>> GenerateRandomAnswerAsync();
    }
}
