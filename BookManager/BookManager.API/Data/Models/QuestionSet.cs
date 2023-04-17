using System.ComponentModel.DataAnnotations.Schema;

namespace BookManager.API.Data.Models
{
    public class QuestionSet
    {
        public Guid Id { get; set; }
        public int SN { get; set; }
        public string? Description { get; set; }

        [ForeignKey(nameof(Chapter))]
        public Guid ChapterId { get; set; }
    }
}
