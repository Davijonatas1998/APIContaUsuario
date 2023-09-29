using APIContaUsuario.Migrations;
using APIContaUsuario.Models;
using APIContaUsuario.Repository.Interface;
using APIContaUsuario.TokenSettings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace APIContaUsuario.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IProfileUser _profileUser;
        private readonly IRoleUser _roleUser;
        private readonly IRefreshToken _refreshToken;

        public AccountController(IProfileUser profileUser, IRoleUser roleUser, IRefreshToken refreshToken)
        {
            _profileUser = profileUser;
            _roleUser = roleUser;
            _refreshToken = refreshToken;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(string Email, string Password)
        {
            var ProfileUser = await _profileUser.DetailsApplicationUser(Email, JwtService.PasswordHash(Password));
            if (ProfileUser == null)
            {
                return Unauthorized("erro");
            }

            var UserManager = await _refreshToken.GetRefreshToken(Email);
            var token = JwtService.GenerateJwtToken(ProfileUser);
            var refreshToken = JwtService.GenerateRefreshToken();
            var date = DateTime.UtcNow;

            if(UserManager == null)
            {
                await _refreshToken.SaveRefreshToken(ProfileUser.Id, date, refreshToken);
            }
            else
            {
                await _refreshToken.EditRefreshToken(ProfileUser.Id, date, refreshToken);
            }

            return Ok(new { Token = token, RefreshToken = refreshToken, Date = date, Message = "login realizado com sucesso" });
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(string ImagemUser, string UserName, string Email, string CPF, string RG, string Numero, string CEP, string Rua, string Bairro, string Cidade, string Estado, string Complemento, string Password)
        {
            var ProfileUser = await _profileUser.GetApplicationUser(Email);
            if (ProfileUser == null)
            {
                var Profile = new ApplicationUser
                {
                    ImagemUser = ImagemUser,
                    UserName = UserName,
                    Email = Email,
                    CPF = CPF,
                    RG = RG,
                    Numero = Numero,
                    CEP = CEP,
                    Rua =   Rua,
                    Bairro = Bairro,
                    Cidade = Cidade,
                    Estado = Estado,
                    Complemento = Complemento,
                    Password = JwtService.PasswordHash(Password)
                };
                await _profileUser.CreateApplicationUser(Profile);
                var RoleUser = new RolesUser
                {
                    RoleAttribute = "Member",
                    Id = Profile.Id
                };

                await _roleUser.CreateRolesUser(RoleUser);
            }
            else
            {
                return Unauthorized("usuario já existente");
            }

            return Ok("cadastro realizado com sucesso");
        }

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken(string Token, string RefreshToken)
        {
            var principal = JwtService.GetClaimsPrincipalExpiredToken(Token);
            var userEmail = principal.FindFirst(ClaimTypes.Email).Value;
            var saveRefreshToken = await _refreshToken.GetRefreshToken(userEmail);

            if (saveRefreshToken != null)
            {
                if (saveRefreshToken.TokenAttribute != RefreshToken)
                {
                    throw new InvalidOperationException("token invalido");
                }
            }

            var date = DateTime.UtcNow;
            var newJWTToken = JwtService.ClaimsJwtToken(principal.Claims);
            var newRefreshToken = JwtService.GenerateRefreshToken();
            await _refreshToken.EditRefreshToken(saveRefreshToken.Id, date, newRefreshToken);

            return Ok(new {Token = newJWTToken, RefreshToken = newRefreshToken, Date = date });
        }

        [HttpPost]
        [Route("RoleManager")]
        public async Task<IActionResult> RoleManager(string email, string refreshToken, string date)
        {
            var ProfileUser = await _profileUser.GetApplicationUser(email);
            var token = JwtService.GenerateJwtToken(ProfileUser);

            return Ok(new { Token = token, RefreshToken = refreshToken, Date = date });
        }

        [HttpGet]
        [Route("Anonymous")]
        [AllowAnonymous]
        public string Anonymous() => "Usuario anônimo";

        [HttpGet]
        [Route("Authenticated")]
        [Authorize]
        public string Authenticated() => String.Format("Autenticado - {0}", User.Identity.Name);

        [HttpGet]
        [Route("Member")]
        [Authorize(Roles = "Member")]
        public string Member() => "Membro logado com sucesso";

        [HttpGet]
        [Route("Admin")]
        [Authorize(Roles = "Admin")]
        public string Admin() => "Administrador logado com sucesso";
    }
}