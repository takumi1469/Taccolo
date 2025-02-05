using System.ComponentModel.DataAnnotations.Schema;

namespace Bachelor_Thesis_Takumi_Saito.Pages.Data
{
    public class LearningSet
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Title { get; set; } = null;
        public string Input { get; set; }
        public string Translation { get; set; }
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
        public string? UserId { get; set; }

        [NotMapped]
        public ApplicationUser User { get; set; }
        public LearningSet(string? title, string input, string translation, string sourceLanguage, string targetLanguage, string? userId = null)
        {
            Title = title;
            Input = input;
            Translation = translation;
            SourceLanguage = sourceLanguage;
            TargetLanguage = targetLanguage;
            UserId = userId;
        }
    }
}
