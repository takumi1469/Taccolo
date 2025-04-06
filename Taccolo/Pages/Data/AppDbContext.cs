using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Taccolo.Pages.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        //This is where you tell the database what to be saved in the database
        public DbSet<LearningSet> LearningSets { get; set; } // Table in the database
        public DbSet<WordMeaningPair> WordMeaningPairs { get; set; } // Table in the database
        public DbSet<Comment> Comments { get; set; } // Table in the database
        public DbSet<HelpRequest> HelpRequests { get; set; } // Table in the database
        public DbSet<HelpReply> HelpReplys { get; set; } // Table in the database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure one-to-many relationship between LearningSet and WordMeaningPair
            modelBuilder.Entity<WordMeaningPair>()
                .HasOne(wmp => wmp.LearningSet) // Each WordMeaningPair has one LearningSet
                .WithMany(ls => ls.WordMeaningPairs) // Each LearningSet has many WordMeaningPairs
                .HasForeignKey(wmp => wmp.LsId) // Foreign key in WordMeaningPair
                .OnDelete(DeleteBehavior.Cascade); // Optional: Specify cascade delete behavior

            modelBuilder.Entity<Comment>()
                .HasOne(comment => comment.LearningSet) 
                .WithMany(ls => ls.Comments) 
                .HasForeignKey(comment => comment.LsId) 
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<HelpRequest>()
                .HasOne(helpRequest => helpRequest.LearningSet)
                .WithMany(ls => ls.HelpRequests)
                .HasForeignKey(helpRequest => helpRequest.LsId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<HelpReply>()
                .HasOne(helpReply => helpReply.HelpRequest)
                .WithMany(helpRequest => helpRequest.HelpReplys)
                .HasForeignKey(helpReply => helpReply.RequestId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
