using Taccolo.Pages.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace Taccolo
{
    public class WordMeaningPair
    {
        public Guid Id { get; set; } = Guid.NewGuid();// Primary key
        public Guid? LsId { get; set; } // Foreign key to LearningSet
        public string? Word { get; set; }
        public string? TranslatedText { get; set; }
        public List<string>? Alternatives { get; set; } = new List<string>();

        public int? Order { get; set; }  

        [NotMapped]
        public LearningSet LearningSet { get; set; } // Navigation property
        public WordMeaningPair() { }
        public WordMeaningPair(Guid lsid, string word, string translatedText, List<string>? alternatives, int order)
        {
            LsId = lsid;
            Word = word;
            TranslatedText = translatedText;
            Alternatives = alternatives;
            Order = order;  
        }
    }
}
