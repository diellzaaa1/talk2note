namespace talk2note.Domain.Entities
{
    public class NoteToDo
    {
        public int Id { get; set; }           
        public string Title { get; set; }      
        public string Description { get; set; }
        public DateTime DueDate { get; set; } 
        public bool IsCompleted { get; set; }   
        public bool NotificationSent { get; set; } 
        public int UserId { get; set; }     
        public User User { get; set; }
    }


}
