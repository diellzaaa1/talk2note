﻿using talk2note.Application.DTO.Note;
using talk2note.Application.Interfaces;
using talk2note.Domain.Entities;

namespace talk2note.Application.Services.NoteService
{
    public class NoteService : INoteService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NoteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
            return true;
        }

        public async Task<bool> DeleteNoteAsync(int id)
        {
            return await _unitOfWork.Notes.DeleteAsync(id);
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
    }
}
