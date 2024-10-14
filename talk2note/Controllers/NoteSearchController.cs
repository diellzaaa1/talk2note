using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using talk2note.Application.Interfaces;
using talk2note.Domain.Entities;

namespace talk2note.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NoteSearchController : ControllerBase
    {
        private readonly INoteSearch _noteSearch;

        public NoteSearchController(INoteSearch noteSearch)
        {
            _noteSearch = noteSearch;
        }

        [HttpGet("{userId}/search")]
        public async Task<ActionResult<List<Note>>> SearchUserNotes(int userId, [FromQuery] string searchTerm, [FromQuery] string additionalFields = null)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest("Search term cannot be empty.");
            }

            string[] additionalFieldArray = additionalFields?.Split(',') ?? new string[] { };

            var notes = await _noteSearch.SearchUserNotesAsync(userId, searchTerm, additionalFieldArray);

            if (notes == null || notes.Count == 0)
            {
                return NotFound("No notes found for the specified user.");
            }

            return Ok(notes); 
        }

    }
}
