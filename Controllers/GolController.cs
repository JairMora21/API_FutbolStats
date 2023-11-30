using API_FutbolStats.Models.DtoCreate;
using API_FutbolStats.Models.DtoUpdate;
using API_FutbolStats.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using API_FutbolStats.Service.Interfaz;

namespace API_FutbolStats.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GolController : ControllerBase
    {
        private readonly IGolService _service;

        public GolController(IGolService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("GolesPorPartido/{idPartido}")]
        public async Task<IActionResult> GolesPorPartido(int idPartido)
        {
            APIResponse response = await _service.GetGolesPartido(idPartido);

            return Ok(response);
        }

        [HttpGet]
        [Route("GolesJugadorPorTemp/{idJugador}/{idTemporada}")]
        public async Task<IActionResult> GolesJugadorPorTemp(int idPartido, int idTemporada)
        {
            APIResponse response = await _service.GetGolesJugadorPorTemporada(idTemporada, idPartido);

            return Ok(response);
        }

        [HttpGet]
        [Route("Gol/{id}")]
        public async Task<IActionResult> Gol(int id)
        {
            APIResponse response = await _service.GetGolById(id);

            return Ok(response);
        }

        [HttpPost]
        [Route("AddGol")]
        public async Task<IActionResult> AddGol([FromBody] GolesDtoCreate golDto)
        {
            APIResponse response = new APIResponse();

            if (ModelState.IsValid)
            {
                response = await _service.AddGol(golDto);
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
            APIResponse response = await _service.DeleteGol(id);

            return Ok(response);
        }

        [HttpPut]
        [Route("UpdateGol/{id}")]
        public async Task<IActionResult> UpdatePartido([FromBody] GolesDtoUpdate golDto, int id)
        {
            APIResponse response = new APIResponse();
            if (ModelState.IsValid && id != 0)
            {
                response = await _service.UpdateGol(golDto, id);
                return Ok(response);
            }

            return BadRequest(response);
        }

    }
}
