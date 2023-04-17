using BookManager.API.Models;

namespace BookManager.API.ServiceProvider
{
    public interface IChapterManager
    {
        //Creating nad Moving
        Task CreateChapter(Guid bookId, Chapter chapter, Guid? chapterId = null);
        Task MoveChapter(Guid tragetBookId, Guid chapterId, Guid? targetChapterId = null);

        //Getting Chapter
        Task<Chapter> GetChapterById(Guid chapterId);
        Task<IEnumerable<Chapter>> GetAllChapter(Guid bookId, Guid? ChapterId = null);

        //Removing Chapter
        Task DeleteChapter(Guid chapterId);

        //Update Chapter
        Task ChangeChapterDescription(Guid chapterId,  string newdescription);
    }
}
