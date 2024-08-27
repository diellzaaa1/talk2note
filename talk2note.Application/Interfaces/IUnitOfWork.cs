
using talk2note.Domain.Entities;

namespace talk2note.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Note> Notes { get; }
        IGenericRepository<User> Users { get; }
        Task<int> CommitAsync();
       
    }

}
