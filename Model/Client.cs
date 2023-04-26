using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Homework12.Model
{
    public class Client
    {
        [Key]
        public int Id { get; set; }
        public string? NameFirst { get; set; }
        public string? NameLast { get; set; }
        public List<Account>? Accounts { get; set; }
    }
}
