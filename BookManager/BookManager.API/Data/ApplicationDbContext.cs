using BookManager.API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BookManager.API.Data
{
    public class ApplicationDbContext
        :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        //Entities
        public DbSet<Book> Books { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<QuestionSet> QuestionSets { get; set; }
    }
}
