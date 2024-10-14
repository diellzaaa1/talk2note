using Elasticsearch.Net;
using talk2note.Application.DTO.Note;
using talk2note.Application.Interfaces;
using talk2note.Domain.Entities;

namespace talk2note.Application.Services.NoteService
{
    public class NoteService : INoteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IElasticsearchService _elasticsearchService;

        public NoteService(IUnitOfWork unitOfWork,IElasticsearchService elasticsearchService)
        {
            _unitOfWork = unitOfWork;
            _elasticsearchService = elasticsearchService;

        }

        public async Task<Note> GetNoteByIdAsync(int id)
        {
            return await _unitOfWork.Notes.GetByIdAsync(id);
        }
  
        public async Task<Note> AddNoteAsync(NoteDTO noteDto)
        {
            var note = new Note
            {
                Title = noteDto.Title,
                Content = noteDto.Content,
                CreatedAt = DateTime.UtcNow,
                UserId = noteDto.UserId,
                FolderId = noteDto.FolderId,
                UpdatedAt = DateTime.UtcNow,
                IsArchived = noteDto.IsArchived,
            };

            await _unitOfWork.Notes.AddAsync(note);
            await _unitOfWork.CommitAsync();
            await _elasticsearchService.IndexDocumentAsync(note);

            return note;
        }

        public async Task<bool> UpdateNoteAsync(int id, NoteUpdateDto updateNoteDto)
        {
            var note = await _unitOfWork.Notes.GetByIdAsync(id);
            if (note == null) return false;

            note.Title = updateNoteDto.Title;
            note.Content = updateNoteDto.Content;
            note.IsArchived = updateNoteDto.IsArchived;
            note.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Notes.Update(note);
            await _unitOfWork.CommitAsync();
            await _elasticsearchService.IndexDocumentAsync(note);

            return true;
        }

        public async Task<bool> DeleteNoteAsync(int id)
        {
            await _unitOfWork.Notes.DeleteAsync(id);

            await _unitOfWork.CommitAsync();

            await _elasticsearchService.DeleteDocumentAsync(id);

            return true; 
        }



        public async Task<IEnumerable<Note>> GetAllNotesAsync()
        {
            return await _unitOfWork.Notes.GetAllAsync();
        }

        public async Task<IEnumerable<Note>> GetNotesByUserIdAsync(int userId)
        {
            return await _unitOfWork.Notes.GetByUserIdAsync(userId);
        }

        public async Task<bool> UpdateNoteFolderAsync(int id, int folderId)
        {
            var existingNote = await _unitOfWork.Notes.GetByIdAsync(id);
            if (existingNote == null) return false;

            var folderExists = await _unitOfWork.Folders.GetByIdAsync(folderId);
            if (folderExists == null) return false;

            existingNote.FolderId = folderId;
            _unitOfWork.Notes.Update(existingNote);

            await _unitOfWork.CommitAsync();
            await _elasticsearchService.IndexDocumentAsync(existingNote);

            return true;
        }

        public async Task<IEnumerable<Note>> GetArchivedNotesByUserIdAsync(int userId)
        {
            return await _unitOfWork.Notes.GetArchivedNotesAsync(userId);
        }

        public async Task<IEnumerable<Note>> GetNotesByFolderIdAsync(int folderId)
        {
            return await _unitOfWork.Notes.GetByFolderIdAsync(folderId);
        }

        public async Task LockNoteAsync(Note note, string password)
        {
            note.Password = BCrypt.Net.BCrypt.HashPassword(password);
             _unitOfWork.Notes.Update(note);
            await _unitOfWork.CommitAsync();    
        }

        public async Task<bool> UnlockNoteAsync(Note note, string password)
        {
            if (note.Password == null) return true; 

            return BCrypt.Net.BCrypt.Verify(password, note.Password);
        }

        public async Task ArchiveNoteAsync(int noteId)
        {
            var note = await _unitOfWork.Notes.GetByIdAsync(noteId);
            if (note == null)
            {
                throw new Exception("Note not found");
            }

            note.IsArchived = true;

            _unitOfWork.Notes.Update(note);
            await _unitOfWork.CommitAsync();
        }
        public async Task AddTagToNoteAsync(int noteId, string tagName, int userId)
        {
            var note = await _unitOfWork.Notes.GetByIdAsync(noteId);
            if (note == null)
            {
                throw new Exception("Note not found");
            }

            var tag = await _unitOfWork.Tags.GetByNameAsync(tagName, userId);
            if (tag == null)
            {
                tag = new Tag
                {
                    Name = tagName,
                    UserId = userId
                };

                await _unitOfWork.Tags.AddAsync(tag);
                await _unitOfWork.CommitAsync();
            }

            var existingNoteTag = await _unitOfWork.NoteTags.GetByNoteAndTagAsync(noteId, tag.TagId);
            if (existingNoteTag != null)
            {
                throw new Exception("Note already has this tag");
            }

            var noteTag = new NoteTag
            {
                NoteId = noteId,
                TagId = tag.TagId
            };

            _unitOfWork.NoteTags.AddAsync(noteTag);
            await _unitOfWork.CommitAsync();
            await _elasticsearchService.IndexDocumentAsync(note);

        }

    }
}
