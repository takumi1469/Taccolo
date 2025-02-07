namespace Bachelor_Thesis_Takumi_Saito.Pages.Data
{
    public class IndividualWordSet
    {
        public Guid Id { get; set; } = Guid.NewGuid();// Primary key
        public Guid LsId { get; set; } // Foreign key to LearningSet
        public List<WordMeaningPair> IndividualWords { get; set; } = new List<WordMeaningPair>();
        public LearningSet LearningSet { get; set; } // Navigation property
    }
}
