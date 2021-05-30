using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialOrganizer.Model
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Required field")]
        [MaxLength(60, ErrorMessage = "Field must contain between 3 and 60 characters")]
        [MinLength(3, ErrorMessage = "Field must contain between 3 and 60 characters")]
        public string Name { get; set; }
        
        [Required]
        public decimal Balance { get; set; }
        
        [Required]
        [ForeignKey("AccountType")]
        public int AccountTypeId { get; set; }
        
        public virtual AccountType AccountType { get; set; }
        
        [Required]
        public bool Status { get; set; }
    }
}