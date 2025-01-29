using Bachelor_Thesis_Takumi_Saito.Pages.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bachelor_Thesis_Takumi_Saito
{
    public class ApplicationUser : IdentityUser
    {
        [NotMapped]
        public ICollection<LearningSet>? LearningSets { get; set; }
        public List<string>? DesiredLanguages { get; set; }
        public List<string>? KnownLanguages { get; set; }
        public string? Bio { get; set; }
    }
}
