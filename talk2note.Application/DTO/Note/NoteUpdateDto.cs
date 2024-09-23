namespace talk2note.Application.DTO.Note
{
    public class NoteUpdateDto
    {
        public int NoteId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsArchived { get; set; }

    }
}
