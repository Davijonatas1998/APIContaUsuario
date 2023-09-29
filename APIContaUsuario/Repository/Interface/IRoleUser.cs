using APIContaUsuario.Models;

namespace APIContaUsuario.Repository.Interface
{
    public interface IRoleUser
    {
        public Task<RolesUser> CreateRolesUser(RolesUser RolesUser);

        public Task<RolesUser> UpdateRolesUser(RolesUser RolesUser);

        public Task<RolesUser> DeleteRolesUser(int? id);
    }
}