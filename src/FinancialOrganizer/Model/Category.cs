using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FinancialOrganizer.Models
{
    public class Category
    {
        [Column("CategoryId")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        
        [Column("CategoryName")]
        [Required(ErrorMessage = "Required field")]
        [MaxLength(60, ErrorMessage = "Field must contain between 3 and 60 characters")]
        [MinLength(3, ErrorMessage = "Field must contain between 3 and 60 characters")]
        public string Name { get; set; }
    }
}