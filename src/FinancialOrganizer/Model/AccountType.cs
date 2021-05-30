using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialOrganizer.Model
{
    public class AccountType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Required field")]
        [MaxLength(60, ErrorMessage = "Field must contain between 3 and 60 characters")]
        [MinLength(3, ErrorMessage = "Field must contain between 3 and 60 characters")]
        public string Name { get; set; }
    }
}