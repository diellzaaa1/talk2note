using Microsoft.EntityFrameworkCore;
using talk2note.Application.Interfaces;
using talk2note.Domain.Entities;
using talk2note.Infrastructure.Data;
using talk2note.Infrastructure.Persistence;

namespace talk2note.Infrastructure.Repositories
{
    public class NoteRepository : GenericRepository<Note>, INoteRepository
    {
        private readonly AppDbContext _context;

        public NoteRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Note>> GetArchivedNotesAsync(int userId)
        {
            return await _context.Notes
                .Where(note => note.UserId == userId && note.IsArchived)
                .ToListAsync();
        }


        public async Task<IEnumerable<Note>> GetByFolderIdAsync(int folderId)
        {
            return await _context.Notes
                .Where(note => note.FolderId == folderId) 
                .ToListAsync();
        }
    }
}
