using API_FutbolStats.Models;
using API_FutbolStats.Models.Dto;
using API_FutbolStats.Models.DtoCreate;
using API_FutbolStats.Models.DtoStats;
using API_FutbolStats.Models.DtoUpdate;
using API_FutbolStats.Service.Interfaz;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace API_FutbolStats.Service.Implementacion
{
    public class PartidoJugadoService : IPartidoJugadoService
    {
        private readonly FutbolStatsContext _context;
        private readonly APIResponse _response;
        private readonly IMapper _mapper;


        public PartidoJugadoService(FutbolStatsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _response = new APIResponse();
        }

        public async Task<APIResponse> GetPartidoJugadorById(int id)
        {
            try
            {
                PartidosJugado jugado = await _context.PartidosJugados.FindAsync(id);

                if (jugado == null || id <= 0)
                {
                    _response.ErrorMessages = new List<string> { "No se encontró el partido" };
                    _response.IsSuccess = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    return _response;
                }
                PartidoJugadoDto jugadoDto = _mapper.Map<PartidoJugadoDto>(jugado);
                _response.Result = jugadoDto;
                _response.statusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;

                return _response;
            }
            catch (Exception ex)
            {
                _response.ErrorMessages = new List<string> { $"Error: {ex.Message}" };
                _response.IsSuccess = false;
                _response.statusCode = HttpStatusCode.InternalServerError;

                return _response;
            }
        }

        public async Task<APIResponse> GetPartidosJugador(int idPartido)
        {
            try
            {
                var partido = await _context.Partidos.AnyAsync(x => x.Id == idPartido);

                if (!partido)
                {
                    _response.ErrorMessages = new List<string> { "No se encontro el partido" };
                    _response.IsSuccess = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    return _response;
                }

                List<PartidoJugadoDtoStats> result = await (from g in _context.PartidosJugados
                                                      join j in _context.Jugadors on g.IdJugador equals j.Id
                                                      where g.IdPartido == 1
                                                      group j by j.Nombre into grouped
                                                      select new PartidoJugadoDtoStats
                                                      {
                                                          Nombre = grouped.Key,
                                                      })
                                                       .ToListAsync();
                _response.Result = result;
                _response.IsSuccess = true;
                _response.statusCode = HttpStatusCode.OK;

                return _response;
            }
            catch (Exception ex)
            {
                _response.ErrorMessages = new List<string> { $"Error: {ex.Message}" };
                _response.IsSuccess = false;
                _response.statusCode = HttpStatusCode.InternalServerError;
                return _response;
            }
        }



        public async Task<APIResponse> AddPartidoJugador(PartidoJugadoDtoCreate partidoDto)
        {
            try
            {
                PartidosJugado jugado = _mapper.Map<PartidosJugado>(partidoDto);

                _context.PartidosJugados.Add(jugado);
                await _context.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.statusCode = HttpStatusCode.OK;

                return _response;
            }
            catch (Exception ex)
            {
                _response.ErrorMessages = new List<string> { $"Error:{ex}" };
                _response.IsSuccess = false;
                _response.statusCode = HttpStatusCode.InternalServerError;

                return _response;
            }
        }

        public async Task<APIResponse> DeletePartidoJugador(int id)
        {
            try
            {
                PartidosJugado jugado = await _context.PartidosJugados.FindAsync(id);

                if (id <= 0 || jugado == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string> { "No se encontró el partido a eliminar o el id es inválido" };
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return _response;
                }

                _context.PartidosJugados.Remove(jugado);
                await _context.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.statusCode = HttpStatusCode.OK;

                return _response;
            }
            catch (Exception ex)
            {
                _response.ErrorMessages = new List<string> { $"Error:{ex}" };
                _response.IsSuccess = false;
                _response.statusCode = HttpStatusCode.InternalServerError;

                return _response;
            }
        }

        public async Task<APIResponse> UpdatePartidoJugador(PartidoJugadoDtoUpdate partidoDto, int id)
        {
            try
            {
                PartidosJugado jugado = await _context.PartidosJugados.FindAsync(id);

                if (jugado == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string> { "No se encontró el partido a actualizar" };
                    _response.statusCode = HttpStatusCode.NotFound;
                    return _response;
                }

                _mapper.Map(partidoDto, jugado);

                _context.PartidosJugados.Update(jugado);
                await _context.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.statusCode = HttpStatusCode.OK;

                return _response;
            }
            catch (Exception ex)
            {
                _response.ErrorMessages = new List<string> { $"Error:{ex}" };
                _response.IsSuccess = false;
                _response.statusCode = HttpStatusCode.InternalServerError;

                return _response;
            }
        }
    }
}
