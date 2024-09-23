using Microsoft.AspNetCore.Mvc;
using talk2note.Application.DTO.NoteToDo;
using talk2note.Application.Services.NoteToDoService;
using talk2note.Domain.Entities;

namespace talk2note.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NoteToDoController : ControllerBase
    {
        private readonly INoteToDoService _noteToDoService;

        public NoteToDoController(INoteToDoService noteToDoService)
        {
            _noteToDoService = noteToDoService;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<NoteToDo>>> GetAll(int userId)
        {
            var notes = await _noteToDoService.GetAllAsync(userId);
            return Ok(notes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NoteToDo>> GetById(int id)
        {
            var noteToDo = await _noteToDoService.GetByIdAsync(id);
            if (noteToDo == null)
            {
                return NotFound("Note not found.");
            }
            return Ok(noteToDo);
        }

        [HttpPost]
        public async Task<ActionResult<NoteToDo>> Create(NoteToDoDTO noteToDoDto)
        {
            var createdNote = await _noteToDoService.CreateAsync(noteToDoDto);
            return CreatedAtAction(nameof(GetById), new { id = createdNote.Id }, createdNote);
        }

        [HttpPut]
        public async Task<IActionResult> Update(NoteToDoUpdateDto noteToDoDto)
        {
            try
            {
                await _noteToDoService.UpdateAsync(noteToDoDto);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound("Note not found.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _noteToDoService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound("Note not found.");
            }
        }

        [HttpPatch("{id}/complete")]
        public async Task<IActionResult> UpdateCompletionStatus(int id, [FromBody] bool isCompleted)
        {
            try
            {
                await _noteToDoService.UpdateCompletionStatusAsync(id, isCompleted);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound("Note not found.");
            }
        }
    }
}
