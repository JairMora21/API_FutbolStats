using API_FutbolStats.Models;
using API_FutbolStats.Models.Custom;
using API_FutbolStats.Service.Interfaz;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace API_FutbolStats.Service.Implementacion
{
    public class AutorizacionService : IAutorizacionService
    {
        private readonly FutbolStatsContext _context;
        private readonly IConfiguration _configuration;

        public AutorizacionService(FutbolStatsContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        private string GenerarToken(string idUsuario)
        {
            var key = _configuration.GetValue<string>("JwtSetttings:key");
            var keyBytes = Encoding.ASCII.GetBytes(key);

            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, idUsuario));

            var credencialesToken = new SigningCredentials(
                new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256Signature
                );

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = credencialesToken
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

            string tokenCreado = tokenHandler.WriteToken(tokenConfig);

            return tokenCreado;
        }

        private string GenerarRefresToken()
        {
            var byteArray = new byte[64];
            var refresToken = "";

            using (var rnd = RandomNumberGenerator.Create())
            {
                rnd.GetBytes(byteArray);
                refresToken = Convert.ToBase64String(byteArray);
            }
            return refresToken;
        }

        private async Task<AutorizacionResponse> GuardarHistorialRefreshToken(int idUsuario, string token, string refreshToken)
        {
            var historialRefreshToken = new HistorialRefreshToken
            {
                IdUsuario = idUsuario,
                Token = token,
                RefreshToken = refreshToken,
                FechaCreacion = DateTime.UtcNow,
                FechaExpiracion = DateTime.UtcNow.AddDays(5)
            };

            DateTime fechaExpiracion = DateTime.UtcNow.AddDays(5);
            string fechaFormateada = fechaExpiracion.ToString("yyyy-MM-dd HH:mm:ss");


            await _context.HistorialRefreshTokens.AddAsync(historialRefreshToken);
            await _context.SaveChangesAsync();

            return new AutorizacionResponse { Token = token, RefreshToken = refreshToken, Resultado = true, Msg="Ok", Expiration = fechaFormateada };
        }


        public async Task<AutorizacionResponse> DevolverToken(AutorizacionRequest autorizacion)
        {
            var usuario_encontrado = _context.Usuarios.FirstOrDefault(x => 
            x.Nombre == autorizacion.NombreUsuario && 
            x.Clave == autorizacion.Clave
            );

            if(usuario_encontrado == null)
            {
                // return await Task.FromResult<AutorizacionResponse>(null);
                return new AutorizacionResponse { Token = null, RefreshToken = null, Resultado = false, Msg = "Usuario / Contraseña incorrecto" };

            }

            string tokenCreado = GenerarToken(usuario_encontrado.Id.ToString());

            string refreshTokenCreado = GenerarRefresToken();

           // return new AutorizacionResponse() { Token = tokenCreado, Resultado = true, Msg="OK" };

            return await GuardarHistorialRefreshToken(usuario_encontrado.Id, tokenCreado, refreshTokenCreado);
        }

        public async Task<AutorizacionResponse> DevolverRefreshToken(RefreshTokenRequest refreshTokenRequest, int idUsuario)
        {
            var refreshTokenEncontrado = _context.HistorialRefreshTokens.FirstOrDefault(x =>
            x.Token == refreshTokenRequest.TokenExpirado &&
            x.RefreshToken == refreshTokenRequest.RefreshToken &&
            x.IdUsuario == idUsuario);

            if(refreshTokenEncontrado == null)
                    return new AutorizacionResponse { Resultado = false, Msg = "No existe refreshToken" };

            var refreshTokenCredo = GenerarRefresToken();
            var tokenCreado = GenerarToken(idUsuario.ToString());

            return await GuardarHistorialRefreshToken(idUsuario, tokenCreado, refreshTokenCredo);
        }
    }
}
