using Microsoft.AspNetCore.Identity.UI.Services;
using talk2note.Application.DTO.Note;
using talk2note.Application.Interfaces;
using talk2note.Application.Services.EmailService;
using talk2note.Domain.Entities;

namespace talk2note.Application.Services.NoteService
{
    public class NoteService : INoteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IElasticsearchService _elasticsearchService;
        private readonly IMailService _emailService;

        public NoteService(IUnitOfWork unitOfWork,IElasticsearchService elasticsearchService,IMailService emailService)
        {
            _unitOfWork = unitOfWork;
            _elasticsearchService = elasticsearchService;
            _emailService = emailService;

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
        public async Task ShareNote(int userId, int noteId, string toEmail)
        {
            // Fetch user from the database
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            // Fetch the note from the database
            var note = await _unitOfWork.Notes.GetByIdAsync(noteId);
            if (note == null)
            {
                throw new Exception("Note not found.");
            }

            // Prepare email subject and message
            string subject = $"A Note Has Been Shared With You: '{note.Title}'";

            string message = $@"
       <html>
    <body style='font-family: Arial, sans-serif; margin: 20px; background-color: #f4f4f4;'>
        <div style='background-color: #ffffff; padding: 20px; border-radius: 8px; box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);'>
           <h2 style='color: #333;'>You’ve Got a Note!</h2>
<p>A note titled <strong>{note.Title}</strong> has been shared with you by <strong>{user.Name}</strong>. Check it out below!</p>

            <div style='padding: 15px; border: 1px solid #ddd; border-radius: 5px; background-color: #f9f9f9;'>
                <h3 style='margin: 0; color: #333;'>{note.Title}</h3>
                <p style='margin: 10px 0; color: #333;'>{note.Content}</p> 
            </div>
          
            <p style='font-size: 12px; color: #777; margin-top: 20px;'>© 2024 Talk2Note. All rights reserved.</p>
        </div>
    </body>
    </html>";

            try
            {
                await _emailService.SendEmailAsync(toEmail, subject, message);
            }
            catch (Exception ex)
            {
               
                throw new Exception("An error occurred while sending the email. Please try again later.", ex);
            }
        }



    }
}
