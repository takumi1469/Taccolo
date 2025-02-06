namespace Bachelor_Thesis_Takumi_Saito
{
    public class WordMeaningPair
    {
        public string Word { get; set; }
        public Meaning MeaningPaired { get; set; }

        public WordMeaningPair() { }

        public WordMeaningPair(string word, Meaning meaningPaired)
        {
            Word = word;
            MeaningPaired = meaningPaired;
        }
    }
}
