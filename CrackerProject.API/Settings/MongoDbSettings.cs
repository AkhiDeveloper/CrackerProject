namespace CrackerProject.API.Settings
{
    public class MongoDbSettings
    {
        public string[] CollectionNames { get; set; } = new string[0];
        public string ConnectionString { get; set; } = String.Empty;
        public string DatabaseName { get; set; } = String.Empty;
    }
}
