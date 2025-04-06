using Taccolo.Pages.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Taccolo
{
    public class ApplicationUser : IdentityUser
    {
        [NotMapped]
        public ICollection<LearningSet>? LearningSets { get; set; } = new List<LearningSet>();
        public List<string>? DesiredLanguages { get; set; } = new List<string>();
        public List<string>? KnownLanguages { get; set; } = new List<string>();
        public string? Bio { get; set; }
    }
}
