using System.ComponentModel.DataAnnotations.Schema;

namespace Bachelor_Thesis_Takumi_Saito.Pages.Data
{
    public class HelpReply
    {
        public string? Body { get; set; }
        public Guid Id { get; set; } = Guid.NewGuid(); // primary key

        // public Guid LsId { get; set; } // HelpReply SHOULD NOT HAVE foreign key to Learning Set
        public string UserId { get; set; } // foreign key to user, User ID in Identity is string type
        public Guid RequestId { get; set; } //foreign key to HelpRequest

        public string Date {  get; set; }

        [NotMapped]
        public HelpRequest HelpRequest { get; set; } // Navigation property

        public HelpReply() { }
        public HelpReply(string body, string userId, Guid requestId, string date) { 

            UserId = userId;
            Body = body;
            RequestId = requestId;      
            Date = date;
        }
    }
}
