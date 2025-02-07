using Bachelor_Thesis_Takumi_Saito.Pages.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bachelor_Thesis_Takumi_Saito
{
    public class WordMeaningPair
    {
        public Guid Id { get; set; } = Guid.NewGuid();// Primary key
        public Guid LsId { get; set; } // Foreign key to LearningSet

        public string Word { get; set; }
        public string TranslatedText { get; set; }
        public List<string>? Alternatives { get; set; } = new List<string>();

        [NotMapped]
        public LearningSet LearningSet { get; set; } // Navigation property
        public WordMeaningPair() { }
        public WordMeaningPair(Guid lsid, string word, string translatedText, List<string>? alternatives)
        {
            LsId = lsid;
            Word = word;
            TranslatedText = translatedText;
            Alternatives = alternatives;
        }
    }
}
