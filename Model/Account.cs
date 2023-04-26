using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Homework12.Model
{
    public class Account
    {
        [Key]
        public int Number { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Sum { get; set; }
        public int ClientId { get; set; }
        public virtual Client? Client { get; set; }
    }
}
