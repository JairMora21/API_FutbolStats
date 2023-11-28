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

    public class TemporadaController : ControllerBase
    {
        private readonly ITemporadaService _service;

        public TemporadaController(ITemporadaService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("Temporadas")]
        public async Task<IActionResult> Temporadas()
        {
            APIResponse response = await _service.GetTemporadas();

            return Ok(response);
        }

        [HttpGet]
        [Route("Temporada/{id}")]
        public async Task<IActionResult> Temporada(int id)
        {
            APIResponse response = await _service.GetTemporadaById(id);

            return Ok(response);
        }

        [HttpPost]
        [Route("AddTemporada")]
        public async Task<IActionResult> AddTemporada([FromBody] TemporadaDtoCreate temporadaDto)
        {
            APIResponse response = new APIResponse();

            if (ModelState.IsValid)
            {
                response = await _service.AddTemporada(temporadaDto);
                return Ok(response);
            }

            response.statusCode = HttpStatusCode.BadRequest;
            response.IsSuccess = false;
   

            return BadRequest(response);
        }

        [HttpDelete]
        [Route("DeleteTemporada/{id}")]
        public async Task<IActionResult> DeleteTemporada(int id)
        {
            APIResponse response = await _service.DeleteTemporada(id);

            return Ok(response);
        }

        [HttpPut]
        [Route("UpdateTemporada/{id}")]
        public async Task<IActionResult> UpdateTemporada([FromBody] TemporadaDtoUpdate temporadaDto, int id)
        {
            APIResponse response = new APIResponse();
            if (ModelState.IsValid && id != 0)
            {
                response = await _service.UpdateTemporada(temporadaDto, id);
                return Ok(response);
            }
      

            return BadRequest(response);
        }
    }
}
