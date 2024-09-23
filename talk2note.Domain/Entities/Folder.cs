using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace talk2note.Domain.Entities
{
    public class Folder
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public int UserId { get; set; } 
        public virtual User User { get; set; }
        public virtual ICollection<Note> Notes { get; set; } = new List<Note>(); 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
    }

}
