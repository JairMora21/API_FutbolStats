using API_FutbolStats.Models.DtoCreate;
using API_FutbolStats.Models.DtoUpdate;
using API_FutbolStats.Models;
using API_FutbolStats.Service.Interfaz;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using API_FutbolStats.Models.Dto;

namespace API_FutbolStats.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartidosJugadorController : ControllerBase
    {

        private readonly IPartidoJugadoService _service;

        public PartidosJugadorController(IPartidoJugadoService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("JugadosPorPartido/{idPartido}")]
        public async Task<IActionResult> JugadosPorPartido(int idPartido)
        {
            APIResponse response = await _service.GetPartidosJugador(idPartido);

            return Ok(response);
        }

        [HttpGet]
        [Route("JugadosJugadorPorTemp/{idJugador}/{idTemporada}")]
        public async Task<IActionResult> JugadosJugadorPorTemp(int idPartido, int idTemporada)
        {
            APIResponse response = await _service.GetPartidosJugadorPorTemporada(idTemporada, idPartido);

            return Ok(response);
        }

        [HttpGet]
        [Route("PartidoJugador/{id}")]
        public async Task<IActionResult> PartidoJugador(int id)
        {
            APIResponse response = await _service.GetPartidoJugadorById(id);

            return Ok(response);
        }

        [HttpPost]
        [Route("AddPartidoJugado")]
        public async Task<IActionResult> AddPartidoJugado([FromBody] PartidoJugadoDtoCreate jugadoDto)
        {
            APIResponse response = new APIResponse();

            if (ModelState.IsValid)
            {
                response = await _service.AddPartidoJugador(jugadoDto);
                return Ok(response);
            }

            response.statusCode = HttpStatusCode.BadRequest;
            response.IsSuccess = false;

            return BadRequest(response);
        }

        [HttpDelete]
        [Route("DeleteGol/{id}")]
        public async Task<IActionResult> DeletePartido(int id)
        {
            APIResponse response = await _service.DeletePartidoJugador(id);

            return Ok(response);
        }

        [HttpPut]
        [Route("UpdateGol/{id}")]
        public async Task<IActionResult> UpdatePartido([FromBody] PartidoJugadoDtoUpdate jugadoDto, int id)
        {
            APIResponse response = new APIResponse();
            if (ModelState.IsValid && id != 0)
            {
                response = await _service.UpdatePartidoJugador(jugadoDto, id);
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
