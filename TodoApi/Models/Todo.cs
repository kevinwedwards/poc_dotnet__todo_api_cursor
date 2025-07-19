using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class Todo
    {
        public int Id { get; set; }
        
        [Required]
        public int Order { get; set; }
        
        [Required]
        public int CreatedByUserId { get; set; }
        
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        
        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        
        public DateTime? PlannedDate { get; set; }
        
        public DateTime? DueDate { get; set; }
    }
} 