using System.ComponentModel.DataAnnotations.Schema;

namespace Taccolo.Pages.Data
{
    public class LearningSet
    {
        public Guid Id { get; set; } = Guid.NewGuid(); //Primary key
        public string? UserId { get; set; } //Foreign key
        public string? Title { get; set; } = null;
        public string Input { get; set; }
        public string Translation { get; set; }
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
        public string? Description { get; set; }
        public string Date {  get; set; }


        [NotMapped]
        public List<WordMeaningPair> WordMeaningPairs { get; set; } = new List<WordMeaningPair>(); // Navigation property

       // [NotMapped]
        public ApplicationUser? User { get; set; }

        [NotMapped]
        public List<Comment> Comments { get; set; }

        [NotMapped]
        public List<HelpRequest> HelpRequests { get; set; }

        public LearningSet() { }
        public LearningSet(string? title, string input, string translation, string sourceLanguage, string targetLanguage, string date, string? description = null, string? userId = null)
        {
            Title = title;
            Input = input;
            Translation = translation;
            SourceLanguage = sourceLanguage;
            TargetLanguage = targetLanguage;
            UserId = userId;
            Date = date;
            Description = description;
        }
    }
}
