using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace talk2note.Domain.Entities
{
    public class NoteTag
    {
        public int NoteId { get; set; } 
        public Note Note { get; set; } 

        public int TagId { get; set; } 
        public Tag Tag { get; set; } 
    }
}
