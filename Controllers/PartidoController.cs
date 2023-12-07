using API_FutbolStats.Models.DtoCreate;
using API_FutbolStats.Models.DtoUpdate;
using API_FutbolStats.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using API_FutbolStats.Service.Interfaz;
using API_FutbolStats.Models.Dto;
using Microsoft.AspNetCore.Authorization;

namespace API_FutbolStats.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartidoController : Controller
    {

        private readonly IPartidoService _service;

        public PartidoController(IPartidoService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("Partidos")]
        public async Task<IActionResult> Partidos()
        {
            APIResponse response = await _service.GetPartidos();

            return Ok(response);
        }

        [HttpGet]
        [Route("Partido/{id}")]
        public async Task<IActionResult> Partido(int id)
        {
            APIResponse response = await _service.GetPartidoById(id);

            return Ok(response);
        }

        [HttpPost]
        [Route("AddPartido")]
        [Authorize]
        public async Task<IActionResult> AddPartido([FromBody] PartidoDtoCreate partidoDto)
        {
            APIResponse response = new APIResponse();

            if (ModelState.IsValid)
            {
                response = await _service.AddPartido(partidoDto);
                return Ok(response);
            }

            response.statusCode = HttpStatusCode.BadRequest;
            response.IsSuccess = false;

            return BadRequest(response);
        }

        [HttpDelete]
        [Route("DeletePartido/{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePartido(int id)
        {
            APIResponse response = await _service.DeletePartido(id);

            return Ok(response);
        }

        [HttpPut]
        [Route("UpdatePartido/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdatePartido([FromBody] PartidoDtoUpdate partidoDto, int id)
        {
            APIResponse response = new APIResponse();
            if (ModelState.IsValid && id != 0)
            {
                response = await _service.UpdatePartido(partidoDto, id);
                return Ok(response);
            }


            return BadRequest(response);
        }
    }
}
