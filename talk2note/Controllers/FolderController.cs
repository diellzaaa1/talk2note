using Microsoft.AspNetCore.Mvc;
using talk2note.Application.DTO.Folder;
using talk2note.Application.Interfaces;
using talk2note.Domain.Entities;
using talk2note.Infrastructure.Repositories;

namespace talk2note.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FolderController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public FolderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpPost]
        public async Task<IActionResult> AddFolder([FromBody] FolderDTO folderDto)
        {
            if (folderDto == null)
            {
                return BadRequest("Note data is null.");
            }


            var folder = new Folder
            {
               Name = folderDto.Name,
               UserId=folderDto.UserId
            };

            await _unitOfWork.Folders.AddAsync(folder);
            await _unitOfWork.CommitAsync();
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFolderById(int id)
        {
            var folder = await _unitOfWork.Folders.GetByIdAsync(id);
            if (folder == null)
            {
                return NotFound();
            }
            return Ok(folder);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> GetAllFolders()
        {
            var folders = await _unitOfWork.Folders.GetAllAsync();

            if (folders == null || !folders.Any())
            {
                return NotFound("No notes found.");
            }

            return Ok(folders); 
        }

        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<Folder>>> GetFolderByUserId(int id)
        {
            var folders = await _unitOfWork.Folders.GetByUserIdAsync(id);


            if (folders == null || !folders.Any())
            {
                return NotFound("No notes found.");
            }

            return Ok(folders);
        }
    }
}
