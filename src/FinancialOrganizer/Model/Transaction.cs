using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace FinancialOrganizer.Models
{
    public class Transaction
    {
        [Column("Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        
        [Column("Category")]
        [Required(ErrorMessage = "Required Field")]
        [Range(1, 1, ErrorMessage = "Invalid Category")]
        public Category Category { get; set; }
        
        [Column("Type")]
        [Required(ErrorMessage = "Required Type")]
        [Range(1, 1, ErrorMessage = "Invalid Type")]
        public Type Type { get; set; }
        
        [Column("Reference")]
        [Required]
        public DateTime Reference { get; set; }
        
        [Column("Value")]
        [Required]
        public decimal Value { get; set; }
        
        [Column("Date")]
        [Required]
        public DateTime Date { get; set; }
        
        [Column("Info")]
        [AllowNull]
        public string Info { get; set; }
        
        // [Column("FilePath")]
        // [AllowNull]
        //public string[] Attachments { get; set; }
    }
}