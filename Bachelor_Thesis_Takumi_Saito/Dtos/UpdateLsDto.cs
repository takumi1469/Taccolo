namespace Bachelor_Thesis_Takumi_Saito.Dtos
{
    public class UpdateLsDto
    {
        public Guid Id { get; set; }
        public string? OriginalText { get; set; }
        public string? TranslatedText { get; set; }

        public List<WordMeaningPairDto>? WordMeaningPairs { get; set;}
        public List<Guid> WmpsToDelete { get; set; }
    }
}
