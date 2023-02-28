using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;

namespace CrackerProject.API.Data.MongoDb.SchemaSecond.Persistence
{
    public static class MongoDbPersistence
    {
        public static void Configure()
        {
            BookMap.Configure();
            SectionMap.Configure();

            BsonDefaults.GuidRepresentation = GuidRepresentation.CSharpLegacy;
            //BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.CSharpLegacy));

            // Conventions
            var pack = new ConventionPack
                {
                    new IgnoreExtraElementsConvention(true),
                    new IgnoreIfDefaultConvention(true)
                };
            ConventionRegistry.Register("My Solution Conventions", pack, t => true);
        }
    }
}
