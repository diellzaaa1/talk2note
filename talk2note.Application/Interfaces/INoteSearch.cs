using Nest;
using talk2note.Domain.Entities;

namespace talk2note.Application.Interfaces
{
    public interface INoteSearch
    {
        Task<List<Note>> SearchUserNotesAsync(int userId, string searchTerm, string[] additionalFields = null);



}
}
