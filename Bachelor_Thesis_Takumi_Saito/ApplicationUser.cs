using Bachelor_Thesis_Takumi_Saito.Pages.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bachelor_Thesis_Takumi_Saito
{
    public class ApplicationUser : IdentityUser
    {
        [NotMapped]
        public ICollection<LearningSet> LearningSets { get; set; }
    }
}
