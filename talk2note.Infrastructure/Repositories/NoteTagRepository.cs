using Microsoft.EntityFrameworkCore;
using talk2note.Application.Interfaces;
using talk2note.Domain.Entities;
using talk2note.Infrastructure.Data;
using talk2note.Infrastructure.Persistence;

namespace talk2note.Infrastructure.Repositories
{
    public class NoteTagRepository : GenericRepository<NoteTag>, INoteTagRepository
    {
        private readonly AppDbContext _context;

        public NoteTagRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }


        public async Task<NoteTag> GetByNoteAndTagAsync(int noteId, int tagId)
        {
            return await _context.NoteTags
       .Where(nt => nt.NoteId == noteId && nt.TagId == tagId)
       .FirstOrDefaultAsync();
        }
    }
}
