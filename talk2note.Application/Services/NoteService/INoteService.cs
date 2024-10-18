using talk2note.Application.DTO.Note;
using talk2note.Domain.Entities;

namespace talk2note.Application.Services.NoteService
{
 
        public interface INoteService
        {
                Task<Note> GetNoteByIdAsync(int id);
                Task<Note> AddNoteAsync(NoteDTO noteDto);
                Task<bool> UpdateNoteAsync(int id, NoteUpdateDto updateNoteDto);
                Task<bool> DeleteNoteAsync(int id);
                Task<IEnumerable<Note>> GetAllNotesAsync();
                Task<IEnumerable<Note>> GetNotesByUserIdAsync(int userId);
                Task<bool> UpdateNoteFolderAsync(int id, int folderId);
                Task<IEnumerable<Note>> GetArchivedNotesByUserIdAsync(int userId);
                Task<IEnumerable<Note>> GetNotesByFolderIdAsync(int folderId);
                Task<bool> UnlockNoteAsync(Note note, string password);
                Task LockNoteAsync(Note note, string password);
                Task ArchiveNoteAsync(int noteId);
               Task AddTagToNoteAsync(int noteId, string tagName, int userId);
                Task ShareNote(int  userId, int noteId,string toEmail);
         

    }
}

