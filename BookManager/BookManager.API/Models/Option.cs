﻿namespace BookManager.API.Models
{
    public class Option
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Text { get; set; }
        public string ImageUri { get; set; } = string.Empty;
        public bool IsCorrect { get; set; } = false;
    }
}