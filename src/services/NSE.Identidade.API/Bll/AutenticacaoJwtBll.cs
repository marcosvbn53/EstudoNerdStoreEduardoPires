using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NSE.Identidade.API.Extensions;
using NSE.Identidade.API.Models.Response;
using NSE.WebAPI.Core.Identidade;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NSE.Identidade.API.Bll
{
    public class AutenticacaoJwtBll
    {
        public SignInManager<IdentityUser> SignInManager { get; }
        public UserManager<IdentityUser> UserManager { get; }

        private readonly AppSettings _appSettings;


        public AutenticacaoJwtBll(SignInManager<IdentityUser> signInManager,
                                  UserManager<IdentityUser> userManager,
                                  IOptions<AppSettings> appSettings)
        {
            SignInManager = signInManager;
            UserManager = userManager;
            _appSettings = appSettings.Value;
        }

        public async Task<UsuarioRespostaLogin> GerarJwt(string email)
        {
            var user = await UserManager.FindByEmailAsync(email);
            var claims = await UserManager.GetClaimsAsync(user);

            var identityClaims = await ObterClaimsUsuario(claims, user);

            var encodedToken = CodificarToken(identityClaims);

            return ObterRespostaToken(encodedToken, user, claims);
        }

        private async Task<ClaimsIdentity> ObterClaimsUsuario(ICollection<Claim> claims, IdentityUser user)
        {
            var userRoles = await UserManager.GetRolesAsync(user);

            //Abaixo estamos criando Claims específicas para JSON web token
            //Então existe uma anotação pra isso, existe um RFC difinindo
            //Cada RFC tem um descrição informando o que é cada dinicação abaixo Sub, Jti, etc...
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            //informando o Guid.NewGuid estamos definindo um Id para o Token e não para o usuário
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));


            //Vamos adicionar mais duas Claims para definir a data de emissão e
            //a data de expiração do token
            //Vamos precisar de um método, que tem o objetivo para converter uma DateTime no formato
            //DateTimeOffset, e por que estamos utilizando esse método? por que é o padrão o JWT

            //Este defini quando o Token vai expirar
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            //Este defini quando o Token foi emitido
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));


            foreach (var userRole in userRoles)
            {
                //role é um papael 
                //Uma claim é um dado aberto, pode representar um apermissão quanto um dado do usuário
                //Vamos adicionar Claim do tipo role para deixar diferenciar
                claims.Add(new Claim("role", userRole));
            }

            //Agora vamos adiconar todas as claims acima tanto as do usuário quanto as do JWT dentro 
            //do identityClaim

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            return identityClaims;
        }

        private UsuarioRespostaLogin ObterRespostaToken(string encodedToken, IdentityUser user, IList<Claim> claims)
        {
            return new UsuarioRespostaLogin
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiracaoHoras).TotalSeconds,
                UsuarioToken = new UsuarioToken
                {
                    Id = user.Id,
                    Email = user.Email,
                    Claims = claims.Select(px => new UsuarioClaim { Type = px.Type, Value = px.Value })
                }
            };
        }

        private string CodificarToken(ClaimsIdentity identityClaims)
        {
            //Vamos gerar o manipulado do token
            var tokenhandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var token = tokenhandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            return tokenhandler.WriteToken(token);
        }

        

        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
