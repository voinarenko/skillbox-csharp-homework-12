using System.ComponentModel.DataAnnotations;

namespace Homework12.Model
{
    public class Client
    {
        [Key]
        public int Id { get; set; }
        public string? NameFirst { get; set; }
        public string? NameLast { get; set; }

        public Client(int id, string? nameFirst, string? nameLast)
        {
            Id = id;
            NameFirst = nameFirst;
            NameLast = nameLast;
        }
    }
}
