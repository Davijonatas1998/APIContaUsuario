using APIContaUsuario.Contexto;
using APIContaUsuario.Models;
using APIContaUsuario.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace APIContaUsuario.Repository
{
    public class RepositoryBase : IProfileUser, IRoleUser, IRefreshToken
    {
        private readonly AppDbContext _context;

        public RepositoryBase(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ApplicationUser>> ListApplicationUser()
        {
            return await _context.ApplicationUsers.ToListAsync();
        }

        public async Task<ApplicationUser> CreateApplicationUser(ApplicationUser User)
        {
            _context.Add(User);
            await _context.SaveChangesAsync();
            return User;
        }

        public async Task<ApplicationUser> UpdateApplicationUser(ApplicationUser User)
        {
            _context.Update(User);
            await _context.SaveChangesAsync();
            return User;
        }

        public async Task<ApplicationUser> DetailsApplicationUser(string Email, string Password)
        {
            return await _context.ApplicationUsers.Where(c => c.Email == Email).FirstOrDefaultAsync(c => c.Password == Password);
        }

        public async Task<ApplicationUser> GetApplicationUser(string Email)
        {
            return await _context.ApplicationUsers.FirstOrDefaultAsync(c => c.Email == Email);
        }

        public async Task<ApplicationUser> DeleteApplicationUser(string Email)
        {
            var UserProfile = await _context.ApplicationUsers.FirstOrDefaultAsync(c => c.Email == Email);
            if (UserProfile != null)
            {
                _context.ApplicationUsers.Remove(UserProfile);
                await _context.SaveChangesAsync();
            }
            return UserProfile;
        }

        public async Task<RolesUser> CreateRolesUser(RolesUser RolesUser)
        {
            _context.Add(RolesUser);
            await _context.SaveChangesAsync();
            return RolesUser;
        }

        public async Task<RolesUser> UpdateRolesUser(RolesUser RolesUser)
        {
            _context.Update(RolesUser);
            await _context.SaveChangesAsync();
            return RolesUser;
        }

        public async Task<RolesUser> DeleteRolesUser(int? id)
        {
            var RolesUser = await _context.Roles.FindAsync(id);
            if (RolesUser != null)
            {
                _context.Roles.Remove(RolesUser);
                await _context.SaveChangesAsync();
            }
            return RolesUser;
        }

        public async Task<RefreshToken> SaveRefreshToken(int Id, DateTime Data, string Token)
        {
            var refreshToken = new RefreshToken
            {
                DateExpired = Data,
                Id = Id,
                TokenAttribute = Token
            };

            _context.Add(refreshToken);
            await _context.SaveChangesAsync();
            return refreshToken;
        }

        public Task<RefreshToken> GetRefreshToken(string Email)
        {
            return _context.RefreshToken.FirstOrDefaultAsync(c => c.UserProfile.Email == Email);
        }

        public async Task<RefreshToken> EditRefreshToken(int Id, DateTime Data, string Token)
        {
            var NewRefreshtoken = await _context.RefreshToken.FirstOrDefaultAsync(c => c.Id == Id);
            if(NewRefreshtoken != null)
            {
                NewRefreshtoken.DateExpired = Data;
                NewRefreshtoken.TokenAttribute = Token;
                NewRefreshtoken.Id = Id;
            }

            _context.Update(NewRefreshtoken);
            await _context.SaveChangesAsync();
            return NewRefreshtoken;
        }
    }
}
