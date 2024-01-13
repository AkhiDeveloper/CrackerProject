namespace BookManager.API.DTOs
{
    public class OptionSetDTO
    {
        public Guid Id { get; set; }
        public short SN { get; set; }
        public IList<OptionDTO> Options { get; set; }
    }
}
