using Desafio1.DataBaseContext;
using Desafio1.DataModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Desafio1.Resultado;
using System.Diagnostics.Eventing.Reader;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace Desafio1.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class CambioController : ControllerBase
    {
        private readonly MyDataBaseContext _dbcontext;
        private IConfiguration config;

        public CambioController(MyDataBaseContext dbcontext,IConfiguration pconfig)
        {
            _dbcontext = dbcontext;
            this.config = pconfig;
        }
        [Authorize]
        [HttpGet]
        [Route("GetCambio")]
        public async Task<IActionResult> GetCambio(double monto, string monedaOrigen,string monedaDestino)
        {
            try
            {
                if (!_dbcontext.Cambio.Any())
                {
                    List<CambioDataModel> Cambios = new List<CambioDataModel>
                    {
                    new CambioDataModel { MonedaOrigen = "USD", MonedaDestino = "PEN", TipoCambio = 3.9 },
                    new CambioDataModel { MonedaOrigen = "EUR", MonedaDestino = "PEN", TipoCambio = 4.04 },

                    };
                    _dbcontext.Cambio.AddRange(Cambios);
                    await _dbcontext.SaveChangesAsync();
                }

                double tipoCambio = 0;

                // busqueda
                List<CambioDataModel> lista =  await _dbcontext.Cambio.ToListAsync();

                CambioDataModel? buscarCambio = lista.Where(c => c.MonedaOrigen == monedaOrigen && c.MonedaDestino == monedaDestino).FirstOrDefault();

                if (buscarCambio != null)
                {
                    tipoCambio = buscarCambio.TipoCambio;
                }
                //

                //resultado
                ResultadoCambio res = new ResultadoCambio();

                res.CodigoResultado = 1;
                res.Monto = monto;
                res.MontoConCambio = monto * tipoCambio;
                res.MonedaOrigen = monedaOrigen;
                res.MonedaDestino = monedaDestino;
                res.TipoCambio = tipoCambio;


                return Ok(res);
            }
            catch (Exception ex)
            {
                ResultadoCambio res = new ResultadoCambio();

                res.CodigoResultado = 0;
                res.Monto = monto;
                res.MontoConCambio = 0;
                res.MonedaOrigen = monedaOrigen;
                res.MonedaDestino = monedaDestino;
                res.TipoCambio = 0;

                return Ok(res);
            }
            
        }

        [Authorize]
        [HttpPut]
        [Route("PutCambio")]
        public async Task<IActionResult> PutCambio(double tipoCambio, string monedaOrigen, string monedaDestino)
        {
            try
            {
                if (!_dbcontext.Cambio.Any())
                {
                    List<CambioDataModel> Cambios = new List<CambioDataModel>
                    {
                    new CambioDataModel { MonedaOrigen = "USD", MonedaDestino = "PEN", TipoCambio = 3.9 },
                    new CambioDataModel { MonedaOrigen = "EUR", MonedaDestino = "PEN", TipoCambio = 4.04 },

                    };
                    _dbcontext.Cambio.AddRange(Cambios);
                    await _dbcontext.SaveChangesAsync();
                }

                // busqueda
                List<CambioDataModel> lista = await _dbcontext.Cambio.ToListAsync();
                CambioDataModel? buscarCambio = lista.Where(c => c.MonedaOrigen == monedaOrigen && c.MonedaDestino == monedaDestino).FirstOrDefault();

                if (buscarCambio != null)
                {                    
                    buscarCambio.TipoCambio = tipoCambio;
                    await _dbcontext.SaveChangesAsync();
                }

                
                //resultado

                return Ok();
            }
            catch (Exception ex)
            {                
                return NotFound();
            }

        }


        [HttpPost]
        [Route("PostLogin")]
        public async Task<IActionResult> PostLogin(string usuario, string pass)
        {
            string jwtToken = GenerateToken(usuario);

            return Ok( new {  token = jwtToken});
        }

        private string GenerateToken(string nombre)
        {
            var vclaims = new[]
            {
                new Claim(ClaimTypes.Name, nombre),
                new Claim(ClaimTypes.Email, nombre),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("JWT:Key").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken(
                claims: vclaims,
                signingCredentials: creds,
                expires: DateTime.Now.AddMinutes(60));

            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return token;

        }


    }
}
