namespace Bachelor_Thesis_Takumi_Saito.Pages.Data
{
    public class Comment
    {
        public string? Body { get; set; }
        public Guid Id { get; set; } = Guid.NewGuid(); // primary key
        public Guid LsId { get; set; } // foreign key to Learning Set
        public Guid UserId { get; set; } // foreign key to user

        public Comment(string body, Guid lsId, Guid userId)
        {
            LsId = lsId;
            UserId = userId;
            Body = body;
        }

    }
}
