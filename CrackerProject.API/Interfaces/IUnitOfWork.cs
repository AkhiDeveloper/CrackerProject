using System;
using System.Threading.Tasks;

namespace CrackerProject.API.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> Commit();
    }
}