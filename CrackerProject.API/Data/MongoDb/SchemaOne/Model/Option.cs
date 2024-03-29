﻿using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.Data.MongoDb.SchemaOne.Model
{
    public class Option
    {
        [BsonElement("sn")]
        [BsonRequired]
        public int Sn { get; set; }

        [BsonElement("body")]
        [BsonRequired]
        public string Body { get; set; }

        [BsonElement("imageUri")]
        public string? ImageUri { get; set; }

        [BsonElement("isCorrect")]
        [BsonRequired]
        [BsonDefaultValue(false)]
        public bool IsCorrect { get; set; }
    }
}