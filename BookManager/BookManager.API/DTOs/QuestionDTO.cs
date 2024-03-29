﻿namespace BookManager.API.DTOs
{
    public class QuestionDTO
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int SN { get; set; }
        public string Text { get; set; }
        public Stream? Image { get; set; }
        public IList<OptionSetDTO> Options { get; set; } = new List<OptionSetDTO>();
    }
}
