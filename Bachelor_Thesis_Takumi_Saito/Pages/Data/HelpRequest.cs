using System.ComponentModel.DataAnnotations.Schema;

namespace Bachelor_Thesis_Takumi_Saito.Pages.Data
{
    public class HelpRequest
    {
        public string? Body { get; set; }
        public Guid Id { get; set; } = Guid.NewGuid(); // primary key
        public Guid LsId { get; set; } // foreign key to Learning Set

        [NotMapped]
        public LearningSet LearningSet { get; set; } // Navigation property

        [NotMapped]
        public List<HelpReply> HelpReplys { get; set; } // Navigation property
        public HelpRequest() { }
        public HelpRequest(string body, Guid lsId)
        {
            LsId = lsId;
            Body = body;
        }

    }
}
