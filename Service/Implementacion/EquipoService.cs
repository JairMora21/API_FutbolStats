using API_FutbolStats.Models;
using API_FutbolStats.Models.Dto;
using API_FutbolStats.Models.DtoCreate;
using API_FutbolStats.Models.DtoUpdate;
using API_FutbolStats.Service.Interfaz;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace API_FutbolStats.Service.Implementacion
{
    public class EquipoService : IEquipoService
    {
        private readonly FutbolStatsContext _context;
        private readonly APIResponse _response;
        private readonly IMapper _mapper;


        public EquipoService(FutbolStatsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _response = new APIResponse();
        }

        public async Task<APIResponse> GetJugadores()
        {
            try
            {
                IEnumerable<Jugador> jugadores = await _context.Jugadors.ToListAsync();

                if (jugadores == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return _response;
                }

                //Se mapean los jugadores de la lista obtenida anteriormente
                List<JugadorDto> jugadoresDto = jugadores
                    .Select(jugador => _mapper.Map<JugadorDto>(jugador))
                    .ToList();

                _response.Result = jugadoresDto;
                _response.IsSuccess = true;
                _response.statusCode = HttpStatusCode.OK;

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.statusCode = HttpStatusCode.InternalServerError;
                _response.Result = $"Error: {ex.Message}";

                return _response;
            }

        }

        public async Task<APIResponse> GetJugadorById(int id)
        {
            try
            {
                Jugador jugador = await _context.Jugadors.FindAsync(id);

                if (jugador == null || id <= 0)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return _response;
                }

                JugadorDto jugadorDto = _mapper.Map<JugadorDto>(jugador);
                _response.Result = jugadorDto;
                _response.IsSuccess = true;
                _response.statusCode = HttpStatusCode.OK;

                return _response;
            }
            catch (Exception ex)
            {
                // Manejar la excepción de manera específica, registrarla, etc.
                _response.IsSuccess = false;
                _response.statusCode = HttpStatusCode.InternalServerError;
                _response.Result = $"Error: {ex.Message}";

                return _response;
            }

        }


        public async Task<APIResponse> AddJugador(JugadorDtoCreate jugadorDto)
        {
            try
            {
                if (jugadorDto == null)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return _response;
                }

                Jugador jugador = _mapper.Map<Jugador>(jugadorDto);

                _context.Jugadors.Add(jugador);
                await _context.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.statusCode = HttpStatusCode.OK;
                _response.Result = jugadorDto;

                return _response;
            }
            catch (Exception ex)
            {
                // Manejar la excepción de manera específica, registrarla, etc.
                _response.IsSuccess = false;
                _response.statusCode = HttpStatusCode.InternalServerError;
                _response.Result = $"Error al agregar jugador: {ex.Message}";

                return _response;
            }

        }


        public async Task<APIResponse> DeleteJugador(int id)
        {
            try
            {
                Jugador jugador = await _context.Jugadors.FindAsync(id);

                if (jugador == null || id <= 0)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return _response;
                }
                _context.Jugadors.Remove(jugador);
                await _context.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.statusCode = HttpStatusCode.OK;

                return _response;
            }
            catch (Exception ex)
            {
                // Manejar la excepción de manera específica, registrarla, etc.
                _response.IsSuccess = false;
                _response.statusCode = HttpStatusCode.InternalServerError;
                _response.Result = $"Error: {ex.Message}";

                return _response;
            }
        }

        public async Task<APIResponse> UpdateJugador(JugadorDtoUpdate jugadorDto, int id)
        {
            try
            {
                Jugador jugador = await _context.Jugadors.FindAsync(id);

                if (jugador == null || jugadorDto == null || id <= 0)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return _response;
                }

                _mapper.Map(jugadorDto, jugador);

                _context.Jugadors.Update(jugador);
                await _context.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.statusCode = HttpStatusCode.OK;

                return _response;
            }
            catch (Exception ex)
            {
                // Manejar la excepción de manera específica, registrarla, etc.
                _response.IsSuccess = false;
                _response.statusCode = HttpStatusCode.InternalServerError;
                _response.Result = $"Error al actualizar jugador: {ex.Message}";

                return _response;
            }

        }
    }
}
