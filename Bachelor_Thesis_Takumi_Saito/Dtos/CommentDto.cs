namespace Bachelor_Thesis_Takumi_Saito.Dtos
{
    public class CommentDto
    {
        //public string UserId { get; set; } // passing UserId between server and client is not secure 
        public string Body { get; set; }
        public Guid LsId { get; set; }
        public string Date { get; set;}
    }
}
