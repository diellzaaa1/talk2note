using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace talk2note.Domain.Entities
{
    public class Note
    {
        public int NoteId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public int UserId { get; set; } 
        public User User { get; set; }
        public int FolderId { get; set; }
        public Folder Folder { get; set; }
        public bool IsArchived { get; set; } = false;

        public virtual ICollection<NoteTag> NoteTags { get; set; } = new List<NoteTag>();


    }
}
