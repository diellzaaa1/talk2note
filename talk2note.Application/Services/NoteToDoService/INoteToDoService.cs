using talk2note.Application.DTO.NoteToDo;
using talk2note.Domain.Entities;

namespace talk2note.Application.Services.NoteToDoService
{
    public interface INoteToDoService
    {

        Task<IEnumerable<NoteToDo>> GetAllAsync(int userId);
        Task<NoteToDo> GetByIdAsync(int id);
        Task<NoteToDo> CreateAsync(NoteToDoDTO noteToDoDto);
        Task UpdateAsync(NoteToDoUpdateDto noteToDoDto);
        Task DeleteAsync(int id);
        Task UpdateCompletionStatusAsync(int id, bool isCompleted);



}
}