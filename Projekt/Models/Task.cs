using System.ComponentModel.DataAnnotations;

namespace Projekt.Models
{
    public class Task
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Data")]
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan Time { get; set; }

        [Display(Name = "Opis zadania")]
        [Required]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Kategoria zadania")]
        public string TaskCategory { get; set; } = string.Empty;

        [Display(Name = "Czy ukończone?")]
        public bool IsCompleted { get; set; }

        [Required]
        public string UserId { get; set; } = Guid.NewGuid().ToString();
    }
}

