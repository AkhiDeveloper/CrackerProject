
using CrackerProject.API.DataModels;
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
                map.MapMember(x => x.Name).SetIsRequired(true);
            });
        }
    }
}
