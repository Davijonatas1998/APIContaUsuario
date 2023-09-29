using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace APIContaUsuario.Models
{
    public class RolesUser
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        public string RoleAttribute { get; set; }

        public int Id { get; set; }

        [ForeignKey("Id")]
        public virtual ApplicationUser UserProfile { get; set; }
    }
}
