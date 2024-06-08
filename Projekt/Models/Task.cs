using System;
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
        [StringLength(100, ErrorMessage = "Opis zadania nie może mieć więcej niż 100 znaków.")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Kategoria zadania")]
        [StringLength(30, ErrorMessage = "Kategoria zadania nie może mieć więcej niż 30 znaków.")]
        public string TaskCategory { get; set; } = string.Empty;

        [Display(Name = "Czy ukończone?")]
        public bool IsCompleted { get; set; }

        [Required]
        public string UserId { get; set; } = Guid.NewGuid().ToString();
    }
}
