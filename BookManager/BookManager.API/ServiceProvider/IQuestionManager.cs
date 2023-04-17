using BookManager.API.Models;

namespace BookManager.API.ServiceProvider
{
    public interface IQuestionManager
    {
        Task CreateQuestion(string chapterId, int set_no, Question question);

        Task GetAllQuestionOfSet(string chapterId, int set_no);

        Task GetAllQuestionOfAllSetOfChapter(string chapterId);


    }
}
