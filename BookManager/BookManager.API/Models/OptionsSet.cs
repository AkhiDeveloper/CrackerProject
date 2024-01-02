namespace BookManager.API.Models
{
    public class OptionsSet
    {
        public Guid Id { get; set; }
        public short SN { get; set; }
        public IList<Option> Options { get; set; }
    }
}
