using APIContaUsuario.Models;

namespace APIContaUsuario.Repository.Interface
{
    public interface IRefreshToken
    {
        public Task<RefreshToken> SaveRefreshToken(int Id, DateTime Data, string Token);

        public Task<RefreshToken> GetRefreshToken(string Email);

        public Task<RefreshToken> EditRefreshToken(int Id, DateTime Data, string Token);
    }
}
