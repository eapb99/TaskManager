using System.ComponentModel.DataAnnotations;

namespace TaskManagerMVC.Models
{
    public class PendingTask
    {
        public int ID { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.Now;

        public DateTime DueDate { get; set; }

        public bool IsCompleted { get; set; }
    }
}
