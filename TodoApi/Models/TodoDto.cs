using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class CreateTodoDto
    {
        [Required]
        public int Order { get; set; }
        
        [Required]
        public int CreatedByUserId { get; set; }
        
        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        
        public DateTime? PlannedDate { get; set; }
        
        public DateTime? DueDate { get; set; }
    }

    public class UpdateTodoDto
    {
        [Required]
        public int Order { get; set; }
        
        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        
        public DateTime? PlannedDate { get; set; }
        
        public DateTime? DueDate { get; set; }
    }
} 