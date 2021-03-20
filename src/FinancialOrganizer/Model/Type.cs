using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialOrganizer.Models
{
    public class Type
    {
        [Column("Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        
        [Column("Name")]
        [Required(ErrorMessage = "Required Field")]
        [MinLength(3, ErrorMessage = "Field must contain more than 3 characters")]
        [MaxLength(30, ErrorMessage = "Field must contain less than 30 characters")]
        public string Name { get; set; }
    }
}