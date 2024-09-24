using Microsoft.AspNetCore.Mvc;
using talk2note.Application.Interfaces;
using talk2note.Infrastructure.Repositories;

namespace talk2note.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public TagsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;   
                
        }

        [HttpGet("note/{noteId}/user/{userId}")]
        public async Task<IActionResult> GetTagsForNoteByUser(int noteId, int userId)
        {
            var tags = await _unitOfWork.Tags.GetTagsForNoteByUserAsync(userId, noteId);

            if (tags == null || !tags.Any())
            {
                return NotFound(new { Message = "No tags found for this note and user." });
            }

            return Ok(tags);
        }
        [HttpDelete("note/{noteId}/tag/{tagId}")]
        public async Task<IActionResult> RemoveTagFromNote(int noteId, int tagId)
        {
            var result = await _unitOfWork.Tags.RemoveTagFromNoteAsync(tagId, noteId);

            if (!result)
            {
                return NotFound(new { Message = "Tag-Note relationship not found." });
            }

            return NoContent();
        }
        [HttpPut("tag/{tagId}")]
        public async Task<IActionResult> UpdateTagName(int tagId, [FromBody] string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                return BadRequest(new { Message = "Tag name cannot be empty." });
            }

            var tag = await _unitOfWork.Tags.GetByIdAsync(tagId);

            if (tag == null)
            {
                return NotFound(new { Message = "Tag not found." });
            }

            tag.Name = newName;

            var result = await _unitOfWork.CommitAsync();

            if (result <= 0)
            {
                return StatusCode(500, new { Message = "Error updating tag name." });
            }

            return Ok(new { Message = "Tag name updated successfully." });
        }
    }
}
