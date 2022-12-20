using CrackerProject.API.Data.MongoDb.SchemaOne.Model;
using MongoDB.Bson.Serialization;

namespace CrackerProject.API.Data.MongoDb.SchemaOne.Persistence
{
    public class SectionMap
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<Section>(map =>
            {
                map.AutoMap();
                map.SetIsRootClass(true);
                map.SetIgnoreExtraElements(true);
                map.MapIdMember(x => x.Id);
                map.MapMember(x => x.Name).SetIsRequired(true);
            });
            BsonClassMap.RegisterClassMap<BookSection>();
            BsonClassMap.RegisterClassMap<SubSection>();
        }
    }
}
