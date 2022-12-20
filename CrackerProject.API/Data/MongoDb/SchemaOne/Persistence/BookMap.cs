using CrackerProject.API.Data.MongoDb.SchemaOne.Model;
using MongoDB.Bson.Serialization;

namespace CrackerProject.API.Data.MongoDb.SchemaOne.Persistence
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
                map.MapMember(x => x.Name).SetIsRequired(true);
            });
        }
    }
}
