using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace APIContaUsuario.Models
{
    public class RefreshToken
    {
        [Key]
        public int RefreshTokenId { get; set; }

        [Required]
        public string TokenAttribute { get; set; }

        public DateTime DateExpired { get; set; }

        public int Id { get; set; }

        [ForeignKey("Id")]
        public virtual ApplicationUser UserProfile { get; set; }
    }
}
