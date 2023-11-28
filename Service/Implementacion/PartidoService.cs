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
    public class PartidoService : IPartidoService
    {
        private readonly FutbolStatsContext _context;
        private readonly APIResponse _response;
        private readonly IMapper _mapper;


        public PartidoService(FutbolStatsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _response = new APIResponse();
        }

        public async Task<APIResponse> GetPartidoById(int id)
        {
            try
            {
                Partido partido = await _context.Partidos.FindAsync(id);

                if (partido == null || id <= 0)
                {
                    _response.ErrorMessages = new List<string> { "No se encontró la temporada" };
                    _response.IsSuccess = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    return _response;
                }
                PartidoDto partidoDto = _mapper.Map<PartidoDto>(partido);
                _response.Result = partidoDto;
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

        public async Task<APIResponse> GetPartidos()
        {
            try
            {
                IEnumerable<Partido> partido = await _context.Partidos.ToListAsync();

                if (partido == null)
                {
                    _response.ErrorMessages = new List<string> { "No se encontraron los partidos" };
                    _response.IsSuccess = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    return _response;
                }
                List<PartidoDto> partidoDtos = partido
                    .Select(p => _mapper.Map<PartidoDto>(p))
                    .ToList();

                _response.Result = partidoDtos;
                _response.IsSuccess = true;
                _response.statusCode = HttpStatusCode.OK;

                return _response;
            }
            catch (Exception ex)
            {
                _response.ErrorMessages = new List<string> { "Ocurrió un error al procesar la solicitud." };
                _response.IsSuccess = false;
                _response.statusCode = HttpStatusCode.InternalServerError;
                return _response;
            }
        }

        public async Task<APIResponse> AddPartido(PartidoDtoCreate partidoDto)
        {
            try
            {
                Partido partido = _mapper.Map<Partido>(partidoDto);

                _context.Partidos.Add(partido);
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

        public async Task<APIResponse> DeletePartido(int id)
        {
            try
            {
                Partido partido = await _context.Partidos.FindAsync(id);

                if (id <= 0 || partido == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string> { "No se encontró la temporada a eliminar o el id es inválido" };
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return _response;
                }

                _context.Partidos.Remove(partido);
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

        public async Task<APIResponse> UpdatePartido(PartidoDtoUpdate partidoDto, int id)
        {
            try
            {
                Partido partido = await _context.Partidos.FindAsync(id);

                if (partido == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string> { "No se encontró la temporada a actualizar" };
                    _response.statusCode = HttpStatusCode.NotFound;
                    return _response;
                }

                _mapper.Map(partidoDto, partido);

                _context.Partidos.Update(partido);
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
