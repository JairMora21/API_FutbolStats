using API_FutbolStats.Models.DtoCreate;
using API_FutbolStats.Models.DtoUpdate;
using API_FutbolStats.Models;
using API_FutbolStats.Service.Interfaz;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API_FutbolStats.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarjetaController : ControllerBase
    {
        private readonly ITarjetaService _service;

        public TarjetaController(ITarjetaService service)
        {
            _service = service;
        }
        [HttpGet]
        [Route("TarjetaPorPartido/{idPartido}")]
        public async Task<IActionResult> TarjetaPorPartido(int idPartido)
        {
            APIResponse response = await _service.GetTarjetasPartido(idPartido);

            return Ok(response);
        }

        [HttpGet]
        [Route("TarjetaJugadorPorTemp/{idJugador}/{idTemporada}")]
        public async Task<IActionResult> TarjetaJugadorPorTemp(int idPartido, int idTemporada)
        {
            APIResponse response = await _service.GetTarjetasJugadorPorTemporada(idTemporada, idPartido);

            return Ok(response);
        }

        [HttpGet]
        [Route("Tarjeta/{id}")]
        public async Task<IActionResult> Gol(int id)
        {
            APIResponse response = await _service.GetTarjetasById(id);

            return Ok(response);
        }

        [HttpPost]
        [Route("AddTarjeta")]
        public async Task<IActionResult> AddTarjeta([FromBody] TarjetaDtoCreate tarjetaDto)
        {
            APIResponse response = new APIResponse();

            if (ModelState.IsValid)
            {
                response = await _service.AddTarjetas(tarjetaDto);
                return Ok(response);
            }

            response.statusCode = HttpStatusCode.BadRequest;
            response.IsSuccess = false;

            return BadRequest(response);
        }

        [HttpDelete]
        [Route("DeleteTarjeta/{id}")]
        public async Task<IActionResult> DeletePartido(int id)
        {
            APIResponse response = await _service.DeleteTarjetas(id);

            return Ok(response);
        }

        [HttpPut]
        [Route("UpdatePartido/{id}")]
        public async Task<IActionResult> UpdatePartido([FromBody] TarjetaDtoUpdate tarjetaDto, int id)
        {
            APIResponse response = new APIResponse();
            if (ModelState.IsValid && id != 0)
            {
                response = await _service.UpdateTarjetas(tarjetaDto, id);
                return Ok(response);
            }

            return BadRequest(response);
        }

    }
}
