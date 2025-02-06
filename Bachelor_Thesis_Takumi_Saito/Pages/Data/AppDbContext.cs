using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bachelor_Thesis_Takumi_Saito.Pages.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<LearningSet> LearningSets { get; set; } // Table in the database
    }

    //public DbSet<LearningSet> LearningSets { get; set; } // Table in the database
}
}