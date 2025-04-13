using System.ComponentModel.DataAnnotations.Schema;

namespace Taccolo.Pages.Data
{
    public class FavoriteSet
    {
        public Guid Id { get; set; } = Guid.NewGuid(); //Primary key of the record itself
        public Guid LsId { get; set; } // Foreign key to the LS
        public string? UserId { get; set; } //Foreign key to the user who added the LS to favorite

        [NotMapped]
        public LearningSet LearningSet { get; set; } // Navigation property

        [NotMapped]
        public ApplicationUser User { get; set; }

        public FavoriteSet(Guid lsId, string userId)
        {
            LsId = lsId;
            UserId = userId;
        }
    }
}
