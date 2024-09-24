using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace talk2note.Domain.Entities
{
    public class Tag
    {
        public int TagId { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; } 
        public User User { get; set; }
        [JsonIgnore]
        public ICollection<NoteTag> NoteTags { get; set; } 
    }


}
