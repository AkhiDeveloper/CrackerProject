using System;
using System.Threading.Tasks;

namespace CrackerProject.API.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> Commit();
    }
}