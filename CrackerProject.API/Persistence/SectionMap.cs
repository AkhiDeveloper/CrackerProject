using CrackerProject.API.DataModels;
using CrackerProject.API.Models;
using MongoDB.Bson.Serialization;

namespace CrackerProject.API.Persistence
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
