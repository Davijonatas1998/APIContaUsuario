using System.ComponentModel.DataAnnotations;

namespace APIContaUsuario.Models
{
    public class ApplicationUser
    {
        [Key]
        public int Id { get; set; }

        public string ImagemUser { get; set; }

        public string UserName { get; set; }

        public string CPF { get; set; }

        public string RG { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string CEP { get; set; }

        public string Rua { get; set; }

        public string Bairro { get; set; }

        public string Cidade { get; set; }

        public string Estado { get; set; }

        public string Numero { get; set; }

        public string Complemento { get; set; }

        public virtual ICollection<RolesUser> RoleUsers { get; set; }
    }
}
