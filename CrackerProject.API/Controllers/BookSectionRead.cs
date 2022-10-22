using CrackerProject.API.Models;

namespace CrackerProject.API.Controllers
{
    public class BookSectionRead
    {
        public Guid Id { get; set; }
        public int Sn { get; set; }
        public string Name { get; set; }
        public QuestionSet[] QuestionSets { get; set; }
        public BookSection[] BookSections { get; set; }
    }
}
