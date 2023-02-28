using DataModel = CrackerProject.API.Data.MongoDb.SchemaOne.Model;
using System.Linq.Expressions;
using CrackerProject.API.Model.Book;

namespace CrackerProject.API.Interfaces
{
    public interface ISectionRepository : IRepository<Section, Guid>
    {
        //Relation to Book
        Task AddtoBook(Guid bookid, Section section);
        Task MovetoBook(Guid sectionid, Guid destination_book_id);
        Task<IEnumerable<Section>> GetfromBook(Guid bookid);
        Task<IEnumerable<Section>> GetfromBook(Guid bookid, Expression<Func<Section, bool>> predicate);
        Task<bool> IsExistInBook(Guid bookid, Guid sectionid);
        Task RemoveBookSections(Guid bookid, DeleteType deleteType);

        //Relation to Section
        Task AddtoSection(Guid parentsectionid, Section section);
        Task MovetoSection(Guid sectionid, Guid targetsectionid);
        Task<IEnumerable<Section>> GetfromSection(Guid parentsectionid);
        Task<IEnumerable<Section>> GetfromSection(Guid parentsectionid, Expression<Func<Section, bool>> predicate);
        Task<bool> IsExistInSection(Guid parentsectionid, Guid sectionid);
        Task RemoveSubSections(Guid parentsectionid, DeleteType deleteType);

        
    }
}
