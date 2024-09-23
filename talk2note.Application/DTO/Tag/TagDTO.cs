using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using talk2note.Domain.Entities;

namespace talk2note.Application.DTO.Tag
{
    public class TagDTO
    { 
        public string Name { get; set; }
        public int UserId { get; set; }
    }
}
