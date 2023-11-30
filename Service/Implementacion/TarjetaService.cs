using API_FutbolStats.Models;
using API_FutbolStats.Models.Dto;
using API_FutbolStats.Models.DtoCreate;
using API_FutbolStats.Models.DtoUpdate;
using API_FutbolStats.Service.Interfaz;
using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using System.Net;

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
        public async  Task<APIResponse> GetTarjetasById(int id)
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

        public async Task<APIResponse> GetTarjetasJugadorPorTemporada(int idTemporada, int idJugador)
        {
            try
            {
                IEnumerable<Tarjetum> tarjeta= await _context.Tarjeta.Where(x => x.IdTemporada == idTemporada && x.IdJugador == idJugador)
                    .ToListAsync();

                if (tarjeta == null)
                {
                    _response.ErrorMessages = new List<string> { "No se encontraron las tarjetas" };
                    _response.IsSuccess = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    return _response;
                }
                List<TarjetaDto> tarjetaDto = tarjeta
                    .Select(x => _mapper.Map<TarjetaDto>(x))
                    .ToList();

                _response.Result = tarjetaDto;
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

        public async Task<APIResponse> GetTarjetasPartido(int idPartido)
        {
            try
            {
                IEnumerable<Tarjetum> tarjeta = await _context.Tarjeta.Where(x => x.IdPartido == idPartido)
                    .ToListAsync();

                if (tarjeta == null)
                {
                    _response.ErrorMessages = new List<string> { "No se encontraron las tarjetas" };
                    _response.IsSuccess = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    return _response;
                }
                List<TarjetaDto> tarjetaDto = tarjeta
                    .Select(x => _mapper.Map<TarjetaDto>(x))
                    .ToList();

                _response.Result = tarjetaDto;
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
