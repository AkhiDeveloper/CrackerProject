﻿using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.Models
{
    public class Question
    {
        [BsonId]
        public Guid id { get; set; } = Guid.NewGuid();

        [BsonElement("sn")]
        [BsonRequired]
        public int Sn { get; set; } = 1;

        [BsonElement("body")]
        [BsonRequired]
        public string Body { get; set; } = String.Empty;

        [BsonElement("imageUri")]
        public string? ImageUri { get; set; } = null;

        [BsonElement("created_date")]
        [BsonRequired]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
