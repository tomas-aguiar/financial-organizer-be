using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialOrganizer.Models
{
    public class Cost
    {
        [Column("CostId")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        
        [Column("CostName")]
        [Required(ErrorMessage = "Required field")]
        [MaxLength(60, ErrorMessage = "Field must contain between 3 and 60 characters")]
        [MinLength(3, ErrorMessage = "Field must contain between 3 and 60 characters")]
        public string Name { get; set; }
        
        [Column("CategoryId")]
        [Required(ErrorMessage = "Required field")]
        [Range(1, 1, ErrorMessage = "Invalid category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
