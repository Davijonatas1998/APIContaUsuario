using APIContaUsuario.Models;
using System.Reflection.Metadata;

namespace APIContaUsuario.Repository.Interface
{
    public interface IProfileUser
    {
        public Task<List<ApplicationUser>> ListApplicationUser();

        public Task<ApplicationUser> CreateApplicationUser(ApplicationUser User);

        public Task<ApplicationUser> UpdateApplicationUser(ApplicationUser User);

        public Task<ApplicationUser> GetApplicationUser(string Email);

        public Task<ApplicationUser> DetailsApplicationUser(string Email, string Password);

        public Task<ApplicationUser> DeleteApplicationUser(string Email);
    }
}
