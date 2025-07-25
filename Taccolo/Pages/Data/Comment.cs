﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Taccolo.Pages.Data
{
    public class Comment
    {
        public string? Body { get; set; }
        public Guid Id { get; set; } = Guid.NewGuid(); // primary key
        public Guid LsId { get; set; } // foreign key to Learning Set
        public string UserId { get; set; } // foreign key to user, user's ID in Identity is string type
        public string Date {  get; set; }

        [NotMapped]
        public LearningSet LearningSet { get; set; } // Navigation property

        public Comment() { }
        public Comment(string body, Guid lsId, string userId, string date)
        {
            LsId = lsId;
            UserId = userId;
            Body = body;
            Date = date;
        }

    }
}
