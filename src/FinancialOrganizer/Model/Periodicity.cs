using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialOrganizer.Models
{
    public class Periodicity
    {
        [Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        public int Id { get; set; }
        
        [Column("Name")]
        [Required]
        public string Name { get; set; }
        
        [Column("Category")]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Category")]
        public Category Category { get; set; }
    }
}