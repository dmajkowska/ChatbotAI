namespace ChatbotAI.API.Contract.GenerateAnswer
{
    public class GenerateAnswerResponse
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public List<string> SectionList { get; set; }
    }
}
