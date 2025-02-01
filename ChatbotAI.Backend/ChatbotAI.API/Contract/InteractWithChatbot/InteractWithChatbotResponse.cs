namespace ChatbotAI.API.Contract.InteractWithChatbot
{
    public class InteractWithChatbotResponse
    {
        public int Id { get; set; }
        public string Question { get; set; } = string.Empty;
        public List<string> SectionList { get; set; }
    }
}
