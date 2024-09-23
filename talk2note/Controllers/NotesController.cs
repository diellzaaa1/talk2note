using Microsoft.AspNetCore.Mvc;
using talk2note.Application.DTO.Note;
using talk2note.Application.Services.NoteService;
using talk2note.Domain.Entities;

namespace talk2note.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNoteById(int id)
        {
            var note = await _noteService.GetNoteByIdAsync(id);
            return note == null ? NotFound() : Ok(note);
        }

        [HttpPost]
        public async Task<IActionResult> AddNote([FromBody] NoteDTO noteDto)
        {
            if (noteDto == null) return BadRequest("Note data is null.");

            var note = await _noteService.AddNoteAsync(noteDto);
            return CreatedAtAction(nameof(GetNoteById), new { id = note.NoteId }, note);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            var deleted = await _noteService.DeleteNoteAsync(id);
            return deleted ? Ok() : NotFound();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNote(int id, [FromBody] NoteUpdateDto updateNoteDto)
        {
            if (id != updateNoteDto.NoteId) return BadRequest("Note ID mismatch.");

            var updated = await _noteService.UpdateNoteAsync(id, updateNoteDto);
            return updated ? Ok() : NotFound();
        }

        [HttpPatch("{id}/folder")]
        public async Task<IActionResult> UpdateNoteFolder(int id, int folderId)
        {

            var updated = await _noteService.UpdateNoteFolderAsync(id, folderId);
            return updated ? Ok() : NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotes()
        {
            var notes = await _noteService.GetAllNotesAsync();
            return notes == null || !notes.Any() ? NotFound("No notes found.") : Ok(notes);
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetNotesByUserId(int id)
        {
            var notes = await _noteService.GetNotesByUserIdAsync(id);
            return notes == null || !notes.Any() ? NotFound("No notes found.") : Ok(notes);
        }

        [HttpGet("user/{userId}/archived")]
        public async Task<ActionResult<IEnumerable<Note>>> GetArchivedNotesByUserId(int userId)
        {
            var archivedNotes = await _noteService.GetArchivedNotesByUserIdAsync(userId);

            if (archivedNotes == null || !archivedNotes.Any())
            {
                return NotFound("No archived notes found for the specified user.");
            }

            return Ok(archivedNotes);
        }

        [HttpGet("folder/{folderId}")]
        public async Task<ActionResult<IEnumerable<Note>>> GetNotesByFolderId(int folderId)
        {
            var notes = await _noteService.GetNotesByFolderIdAsync(folderId);

            if (notes == null || !notes.Any())
            {
                return NotFound("No notes found for the specified folder.");
            }

            return Ok(notes);
        }
    }
}
