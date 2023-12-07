using API_FutbolStats.Models;
using API_FutbolStats.Models.Dto;
using API_FutbolStats.Models.DtoCreate;
using API_FutbolStats.Models.DtoStats;
using API_FutbolStats.Models.DtoUpdate;
using API_FutbolStats.Service.Interfaz;
using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using System.Net;
using static API_FutbolStats.Service.Implementacion.EquipoService;

namespace API_FutbolStats.Service.Implementacion
{
    public class TarjetaService : ITarjetaService
    {
        private readonly FutbolStatsContext _context;
        private readonly APIResponse _response;
        private readonly IMapper _mapper;


        public TarjetaService(FutbolStatsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _response = new APIResponse();
        }
        public async Task<APIResponse> GetTarjetasById(int id)
        {
            try
            {
                Tarjetum tarjeta = await _context.Tarjeta.FindAsync(id);

                if (tarjeta == null || id <= 0)
                {
                    _response.ErrorMessages = new List<string> { "No se encontró la tarjeta" };
                    _response.IsSuccess = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    return _response;
                }
                TarjetaDto tarjetaDto = _mapper.Map<TarjetaDto>(tarjeta);
                _response.Result = tarjetaDto;
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

        public async Task<APIResponse> GetTarjetasPartido(int idPartido)
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
                List<TarjetaPartidoDtoStats> result = await (from g in _context.Tarjeta
                                                             join j in _context.Jugadors on g.IdJugador equals j.Id
                                                             join tt in _context.TipoTarjeta on g.IdTipoTarjeta equals tt.Id
                                                             where g.IdPartido == idPartido
                                                             select new TarjetaPartidoDtoStats
                                                             {
                                                                 Nombre = j.Nombre,
                                                                 Tarjeta = tt.Tarjeta,
                                                                 idTipoTarjeta = tt.Id
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

        public async Task<APIResponse> AddTarjetas(TarjetaDtoCreate tarjetaDto)
        {
            try
            {
                Tarjetum tarjeta = _mapper.Map<Tarjetum>(tarjetaDto);

                _context.Tarjeta.Add(tarjeta);
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

        public async Task<APIResponse> DeleteTarjetas(int id)
        {
            try
            {
                Tarjetum tarjeta = await _context.Tarjeta.FindAsync(id);

                if (id <= 0 || tarjeta == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string> { "No se encontró la tarjeta a eliminar o el id es inválido" };
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return _response;
                }

                _context.Tarjeta.Remove(tarjeta);
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

        public async Task<APIResponse> UpdateTarjetas(TarjetaDtoUpdate tarjetaDto, int id)
        {
            try
            {
                Tarjetum tarjeta = await _context.Tarjeta.FindAsync(id);

                if (tarjeta == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string> { "No se encontró la tarjeta a actualizar" };
                    _response.statusCode = HttpStatusCode.NotFound;
                    return _response;
                }

                _mapper.Map(tarjetaDto, tarjeta);

                _context.Tarjeta.Update(tarjeta);
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
