using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Homework12.Model
{
    public abstract class Account
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        public int Number { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Sum { get; set; }
        public int ClientId { get; set; }
        public AccountType AccountType { get; set; }
    }
}
