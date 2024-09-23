using talk2note.Application.Interfaces;
using talk2note.Infrastructure.Data;
using talk2note.Infrastructure.Repositories;

namespace talk2note.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public INoteRepository Notes { get; private set; } 
        public IUserRepository Users { get; private set; }
        public IFolderRepository Folders { get; private set; }
        public ITagRepository Tags { get; private set; }

        public INoteToDoRepository NotesToDo { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Notes = new NoteRepository(_context);
            Users = new UserRepository(_context);
            Folders = new FolderRepository(_context);
            Tags = new TagRepository(_context);
            NotesToDo= new NoteToDoRepository(_context);
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
