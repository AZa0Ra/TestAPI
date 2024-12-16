using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;
        [Required]
        [StringLength(150)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty; 

        public int AccountId { get; set; }
        public Account Account { get; set; }
    }
}