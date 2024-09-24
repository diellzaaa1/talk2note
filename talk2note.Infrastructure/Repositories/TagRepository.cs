using Microsoft.EntityFrameworkCore;
using talk2note.Application.Interfaces;
using talk2note.Domain.Entities;
using talk2note.Infrastructure.Data;
using talk2note.Infrastructure.Persistence;

namespace talk2note.Infrastructure.Repositories
{
    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        private readonly AppDbContext _context;

        public TagRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

     
        public async Task<Tag> GetByNameAsync(string tagName, int userId)
        {
            return await _context.Tags.Where(t => t.Name == tagName && t.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Tag>> GetTagsForNoteByUserAsync(int userId, int noteId)
        {
            return await _context.Tags
                .Include(t => t.NoteTags)
                .Where(t => t.UserId == userId && t.NoteTags.Any(nt => nt.NoteId == noteId))
                .ToListAsync();
        }

        public async Task<bool> RemoveTagFromNoteAsync(int tagId, int noteId)
        {
            var noteTag = await _context.NoteTags
                .FirstOrDefaultAsync(nt => nt.TagId == tagId && nt.NoteId == noteId );

            if (noteTag == null)
            {
                return false; 
            }

            _context.NoteTags.Remove(noteTag);
            await _context.SaveChangesAsync();

            return true; 
        }
    }
}
