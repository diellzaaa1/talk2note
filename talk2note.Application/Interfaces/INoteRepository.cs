using talk2note.Domain.Entities;

namespace talk2note.Application.Interfaces
{
    public interface INoteRepository : IGenericRepository<Note>
    {
        Task<IEnumerable<Note>> GetArchivedNotesAsync(int userId);

        Task<IEnumerable<Note>> GetByFolderIdAsync(int folderId);


    }
}
