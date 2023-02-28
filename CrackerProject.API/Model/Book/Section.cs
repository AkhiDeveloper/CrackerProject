using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.Model.Book
{
    public class Section
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public int Sn { get; set; }

        public string Name { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime AddedDate { get; set; } = DateTime.UtcNow;
    }

    public enum DeleteType
    {
        ElementOnly,
        AssociatedAlso
    }
}
