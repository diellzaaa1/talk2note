using Microsoft.AspNetCore.Mvc;
using talk2note.Application.Interfaces;
using talk2note.Domain.Entities;
using talk2note.Application.DTO.Note;
using Microsoft.AspNetCore.Authorization;

namespace talk2note.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNoteById(int id)
        {
            var note = await _unitOfWork.Notes.GetByIdAsync(id);
            if (note == null)
            {
                return NotFound();
            }
            return Ok(note);
        }

        [HttpPost]
        public async Task<IActionResult> AddNote([FromBody] NoteDTO noteDto)
        {
            if (noteDto == null)
            {
                return BadRequest("Note data is null.");
            }

          
            var note = new Note
            {
                Title = noteDto.Title,
                Content = noteDto.Content,
                CreatedAt = DateTime.UtcNow,
                UserId = noteDto.UserId,
                UpdatedAt = DateTime.UtcNow,
                IsArchived = noteDto.IsArchived,
            };

            await _unitOfWork.Notes.AddAsync(note);
            await _unitOfWork.CommitAsync();
            return Ok();
        }
    }
}
