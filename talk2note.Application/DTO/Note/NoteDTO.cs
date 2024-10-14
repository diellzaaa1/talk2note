namespace talk2note.Application.DTO.Note
{
    public class NoteDTO
    {
      
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public int UserId { get; set; }

        public int FolderId { get; set; }
        public bool IsArchived { get; set; } = false;
    }
}
