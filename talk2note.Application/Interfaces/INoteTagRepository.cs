using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using talk2note.Domain.Entities;

namespace talk2note.Application.Interfaces
{
    public interface INoteTagRepository : IGenericRepository<NoteTag>
    {
        Task<NoteTag> GetByNoteAndTagAsync(int noteId, int tagId);


    }
}
