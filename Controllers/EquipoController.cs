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
    public class EquipoController : ControllerBase
    {

        private readonly IEquipoService _service;

        public EquipoController(IEquipoService service)
        {
            _service = service;
        }

        //Aqui se encuentra el CRUD del equipo (Mas relacionado a los jugadores)
        //Tambien estaran los Endpoints de estadisticas

        [HttpGet]
        [Route("Equipos")]
        public async Task<IActionResult> Equipos()
        {

            APIResponse response = await _service.GetJugadores();

            return Ok(response);
        }

        [HttpGet]
        [Route("Equipo/{id}")]
        public async Task<IActionResult> Equipo(int id)
        {

            APIResponse response = await _service.GetJugadores();

            return Ok(response);
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
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> AddJugador(int id)
        {
            APIResponse response = await _service.DeleteJugador(id);

            return Ok(response);
        }

        [HttpPut]
        [Route("UpdateJugador/{id}")]
        [Authorize]
        public async Task<IActionResult> AddJugador([FromBody] JugadorDtoUpdate jugadorDto, int id)
        {
            if (ModelState.IsValid)
            {
                APIResponse response = await _service.UpdateJugador(jugadorDto, id);
                return Ok(response);
            }
            return BadRequest();
        }

        //Estadisticas individuales de un jugador en especifico
        [HttpGet]
        [Route("JugadorStats/{idJugador}/{idTemporada}")]
        public async Task<IActionResult> JugadorStats(int idJugador, int idTemporada)
        {

            APIResponse response = await _service.GetStatsJugador(idJugador, idTemporada);

            return Ok(response);
        }

        //nos dara las estadisticas del equipo ingresado (goles, vistorias, tarjetas etc..)
        [HttpGet]
        [Route("EquipoStats/{idEquipo}")]
        public async Task<IActionResult> EquipoStats(int idEquipo, int? idTemporada = null)
        {

            APIResponse response = await _service.GetStatsEquipo(idEquipo, idTemporada);

            return Ok(response);
        }

        //Nos dara la lista de top goleadores del equipo
        [HttpGet]
        [Route("TopGoleadores/{idEquipo}")]
        public async Task<IActionResult> TopGoleadores(int idEquipo, int? idTemporada = null)
        {
            APIResponse response = await _service.GetStatsGoleador(idEquipo, idTemporada);

            return Ok(response);
        }

        //Nos dara la lista de top que tenga mas partidos con el equipo
        [HttpGet]
        [Route("TopPartidos/{idEquipo}")]
        public async Task<IActionResult> TopPartidos(int idEquipo, int? idTemporada = null)
        {
            APIResponse response = await _service.GetStatsPartidos(idEquipo, idTemporada);

            return Ok(response);
        }

        //Nos dara la lista de top amarillas del equipo
        [HttpGet]
        [Route("TopAmarillas/{idEquipo}")]
        public async Task<IActionResult> TopAmarillas(int idEquipo, int? idTemporada = null)
        {
            APIResponse response = await _service.GetStatsAmarillas(idEquipo, idTemporada);

            return Ok(response);
        }
        //Nos dara la lista de top amarillas del equipo
        [HttpGet]
        [Route("TopRojas/{idEquipo}")]
        public async Task<IActionResult> TopRojas(int idEquipo, int? idTemporada = null)
        {
            APIResponse response = await _service.GetStatsRojas(idEquipo, idTemporada);

            return Ok(response);
        }

    }
}
