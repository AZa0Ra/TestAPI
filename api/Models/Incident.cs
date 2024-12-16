using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class Incident
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public List<Account> Accounts { get; set; } = new List<Account>();
    }
}