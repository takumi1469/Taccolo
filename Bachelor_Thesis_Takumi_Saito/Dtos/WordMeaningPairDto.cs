namespace Bachelor_Thesis_Takumi_Saito.Dtos
{
    public class WordMeaningPairDto
    {
        public Guid Id { get; set; }
        public Guid LsId { get; set; }
        public string Word { get; set; } = string.Empty;
        public string TranslatedText { get; set; } = string.Empty;
        public List<string> Alternatives { get; set; } = new();
        public int Order { get; set; }
    }
}
