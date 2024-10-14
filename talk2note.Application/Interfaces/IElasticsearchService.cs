using Nest;
using talk2note.Domain.Entities;

namespace talk2note.Application.Interfaces
{
    public interface IElasticsearchService
    {
        Task IndexDocumentAsync(Note note);
        Task DeleteDocumentAsync(int noteId);
        Task<Note> GetDocumentAsync(int noteId);
        Task<ISearchResponse<Note>> SearchAsync(string query);
    }
}
