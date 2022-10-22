using CrackerProject.API.Models;
using MongoDB.Bson.Serialization;

namespace CrackerProject.API.Persistence
{
    public class BookSectionMap
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<BookSection>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
                map.MapIdMember(x => x.Id);
                map.MapMember(x => x.Name).SetIsRequired(true);
            });
        }
    }
}
