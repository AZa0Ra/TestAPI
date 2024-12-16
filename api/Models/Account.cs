using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        public List<Contact> Contacts {get; set;} = new List<Contact>();
    }
}