using ShopSiloApp.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodingChallenge.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        [Required]
        [StringLength(50)]
        public string? Username { get; set; }

        [Required]
        [StringLength(100)]
        public string? Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters long.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        public UserRole Role { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [Column(TypeName = "datetime")]
        public DateTime? LastLogin { get; set; }

       
    }
}
