using System;

namespace talk2note.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Note> Notes { get; set; }
        public virtual ICollection<Folder> Folders { get; set; }

        public virtual ICollection<NoteToDo> NotesToDo { get; set; }


    }
}
