using talk2note.Application.DTO.NoteToDo;
using talk2note.Application.Interfaces;
using talk2note.Domain.Entities;
using System;

namespace talk2note.Application.Services.NoteToDoService
{
    public class NoteToDoService : INoteToDoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NoteToDoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<NoteToDo>> GetAllAsync(int userId)
        {
            return await _unitOfWork.NotesToDo.GetByUserIdAsync(userId);
        }

        public async Task<NoteToDo> GetByIdAsync(int id)
        {
            var noteToDo = await _unitOfWork.NotesToDo.GetByIdAsync(id);
            if (noteToDo == null)
            {
                throw new Exception($"NoteToDo with ID {id} not found."); // or return null if preferred
            }
            return noteToDo;
        }

        public async Task<NoteToDo> CreateAsync(NoteToDoDTO noteToDoDto)
        {
            var noteToDo = new NoteToDo
            {
                Title = noteToDoDto.Title,
                Description = noteToDoDto.Description,
                DueDate = noteToDoDto.DueDate,
                UserId = noteToDoDto.UserId
            };

            await _unitOfWork.NotesToDo.AddAsync(noteToDo);
            await _unitOfWork.CommitAsync();
            return noteToDo;
        }

        public async Task UpdateAsync(NoteToDoUpdateDto noteToDoDto)
        {
            var noteToDo = await _unitOfWork.NotesToDo.GetByIdAsync(noteToDoDto.Id);
            if (noteToDo == null)
            {
                throw new Exception($"NoteToDo with ID {noteToDoDto.Id} not found.");
            }

            noteToDo.Title = noteToDoDto.Title;
            noteToDo.Description = noteToDoDto.Description;
            noteToDo.DueDate = noteToDoDto.DueDate;
            noteToDo.IsCompleted = noteToDoDto.IsCompleted;

            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var noteToDo = await _unitOfWork.NotesToDo.GetByIdAsync(id);
            if (noteToDo == null)
            {
                throw new Exception($"NoteToDo with ID {id} not found.");
            }

            await _unitOfWork.NotesToDo.DeleteAsync(id);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateCompletionStatusAsync(int id, bool isCompleted)
        {
            var noteToDo = await _unitOfWork.NotesToDo.GetByIdAsync(id);
            if (noteToDo == null)
            {
                throw new Exception($"NoteToDo with ID {id} not found.");
            }

            noteToDo.IsCompleted = isCompleted;

            await _unitOfWork.CommitAsync();
        }
    }
}
