using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.Models
{
    public class BookSection
    {
        [BsonId]
        public Guid Id { get; set; } = Guid.NewGuid();

        [BsonElement("sn")]
        public int Sn { get; set; }
        //{
        //    get
        //    {
        //        return Sn;
        //    }
        //    set 
        //    {
        //        if (value < 1) 
        //        { 
        //            Sn = 1;
        //        }
        //    } 
        //}

        [BsonElement("name")]
        [BsonRequired]
        public string Name { get; set; }

        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;

        [BsonElement("added_date")]
        [BsonRequired]
        public DateTime AddedDate { get; set; } = DateTime.UtcNow;

        [BsonElement("book_id")]
        [BsonRequired]
        public Guid BookId { get; set; }

        [BsonElement("parentSection_id")]
        public Guid ParentBookSectionId { get; set; } = Guid.Empty;

        [BsonElement("questionSets")]
        public QuestionSet[] QuestionSets { get; set; }

    }
}
