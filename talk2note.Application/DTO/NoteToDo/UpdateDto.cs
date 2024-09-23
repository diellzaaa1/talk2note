namespace talk2note.Application.DTO.NoteToDo
{

    public class NoteToDoUpdateDto
        {
            public int Id { get; set; } 
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime DueDate { get; set; }
            public bool IsCompleted { get; set; }
        }
    

}
