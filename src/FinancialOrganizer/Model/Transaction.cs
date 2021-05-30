using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialOrganizer.Model
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }

        [Required]
        public decimal Amount { get; set; }
        
        [Required]
        public DateTime TimeStamp { get; set; }
        
        [Required(ErrorMessage = "Required field")]
        [MaxLength(60, ErrorMessage = "Field must contain between 3 and 60 characters")]
        [MinLength(3, ErrorMessage = "Field must contain between 3 and 60 characters")]
        public string Description { get; set; }
        
        [Required]
        [ForeignKey("Account")]
        public int AccountId { get; set; }
        
        public virtual Account Account { get; set; }
        
        [Required]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        
        public virtual Category Category { get; set; }
        
        [Required]
        public bool IsBudget { get; set; }
        
        // [Column("FilePath")]
        // [AllowNull]
        // public string[] Attachments { get; set; }
    }
}