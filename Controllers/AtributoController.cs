using API_FutbolStats.Models;
using API_FutbolStats.Service.Interfaz;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_FutbolStats.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AtributoController : ControllerBase
    {
        private readonly IAtributosService _atributos;

        public AtributoController(IAtributosService atributo)
        {
            _atributos = atributo;
        }
        [HttpGet]
        [Route("Tarjetas")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Tarjetas()
        {
            APIResponse response = await _atributos.GetTipoTarjetas();

            return Ok(response);
        }

        [HttpGet]
        [Route("Posiciones")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Posiciones()
        {
            APIResponse response = await _atributos.GetPosicionesJugador();

            return Ok(response);
        }

        [HttpGet]
        [Route("Partidos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Partidos()
        {
            APIResponse response = await _atributos.GetTipoPartido();

            return Ok(response);
        }

        [HttpGet]
        [Route("Clasificacion")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Clasificacion()
        {
            APIResponse response = await _atributos.GetClasificacion();
       
            return Ok(response);
        }
    }
}
