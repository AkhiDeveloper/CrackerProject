﻿using CrackerProject.API.Model;
using CrackerProject.API.Models;
using MongoDB.Bson.Serialization;

namespace CrackerProject.API.Persistence
{
    public class BookMap
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<Book>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
                map.MapIdMember(x => x.Id);
                map.MapMember(x => x.Description).SetIsRequired(true);
            });
        }
    }
}