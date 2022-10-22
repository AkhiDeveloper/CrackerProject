using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.ViewModel
{
    public class BookResponse
    {
        public Guid? Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; } 

        public DateTime? CreatedDateTime { get; set; } 

        public string? Publisher { get; set; }
        
        public string? author { get; set; } 

        public int? Edition { get; set; }
    }
}
