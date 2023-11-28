using API_FutbolStats.Models;
using API_FutbolStats.Models.DtoCreate;
using API_FutbolStats.Models.DtoUpdate;
using API_FutbolStats.Service.Interfaz;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_FutbolStats.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class EquipoController : ControllerBase
    {

        private readonly IEquipoService _service;

        public EquipoController(IEquipoService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("Jugadores")]
        public async Task<IActionResult> Jugadores()
        {

            APIResponse response = await _service.GetJugadores();

            return Ok(response);
        }

        [HttpGet]
        [Route("Jugador/{id}")]
        public async Task<IActionResult> Jugador(int id)
        {
            APIResponse response = await _service.GetJugadorById(id);

            return Ok(response);
        }

        [HttpPost]
        [Route("AddJugador")]
        public async Task<IActionResult> AddJugador([FromBody] JugadorDtoCreate jugadorDto)
        {
            if (ModelState.IsValid)
            {
                APIResponse response = await _service.AddJugador(jugadorDto);
                return Ok(response);
            }
            return BadRequest();

        }

        [HttpDelete]
        [Route("DeleteJugador/{id}")]
        public async Task<IActionResult> AddJugador(int id)
        {
            APIResponse response = await _service.DeleteJugador(id);

            return Ok(response);
        }

        [HttpPut]
        [Route("UpdateJugador")]
        public async Task<IActionResult> AddJugador([FromBody] JugadorDtoUpdate jugadorDto, int id)
        {
            if (ModelState.IsValid)
            {
                APIResponse response = await _service.UpdateJugador(jugadorDto, id);
                return Ok(response);
            }
            return BadRequest();
        }
    }
}
