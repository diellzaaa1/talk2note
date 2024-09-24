using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using talk2note.Domain.Entities;

namespace talk2note.Application.Interfaces
{
    public interface ITagRepository: IGenericRepository<Tag>
    {
        Task<Tag> GetByNameAsync(string tagName, int userId);
        Task<IEnumerable<Tag>> GetTagsForNoteByUserAsync(int userId, int noteId);
        Task<bool> RemoveTagFromNoteAsync(int tagId, int noteId);
    }
}
