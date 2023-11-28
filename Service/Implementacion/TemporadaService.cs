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
    public class TemporadaService : ITemporadaService
    {

        private readonly FutbolStatsContext _context;
        private readonly APIResponse _response;
        private readonly IMapper _mapper;


        public TemporadaService(FutbolStatsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _response = new APIResponse();
        }
        public async Task<APIResponse> GetTemporadaById(int id)
        {
            try
            {
                Temporadum temporadum = await _context.Temporada.FindAsync(id);

                if (temporadum == null || id <= 0)
                {
                    _response.ErrorMessages = new List<string> { "No se encontró la temporada" };
                    _response.IsSuccess = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    return _response;
                }
                TemporadaDto temporadaDto = _mapper.Map<TemporadaDto>(temporadum);
                _response.Result = temporadaDto;
                _response.statusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;

                return _response;
            }
            catch (Exception ex)
            {
                // Manejo de la excepción. Puedes registrar el error, enviar notificaciones, etc.
                _response.ErrorMessages = new List<string> { "Ocurrió un error al procesar la solicitud." };
                _response.IsSuccess = false;
                _response.statusCode = HttpStatusCode.InternalServerError;

                // Aquí podrías registrar el error o imprimirlo en la consola.
                Console.WriteLine($"Error: {ex.Message}");

                return _response;
            }
        }

        public async Task<APIResponse> GetTemporadas()
        {
            try
            {
                IEnumerable<Temporadum> temporadas = await _context.Temporada.ToListAsync();

                if (temporadas == null)
                {
                    _response.ErrorMessages = new List<string> { "No se encontraron las temporadas" };
                    _response.IsSuccess = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    return _response;
                }
                List<TemporadaDto> temporadaDtos = temporadas
                    .Select(temp => _mapper.Map<TemporadaDto>(temp))
                    .ToList();

                _response.Result = temporadaDtos;
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

        public async Task<APIResponse> AddTemporada(TemporadaDtoCreate temporadaDto)
        {
            try
            {
                Temporadum temporada = _mapper.Map<Temporadum>(temporadaDto);

                _context.Temporada.Add(temporada);
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

        public async Task<APIResponse> DeleteTemporada(int id)
        {
            try
            {
                Temporadum temporada = await _context.Temporada.FindAsync(id);

                if (id <= 0 || temporada == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string> { "No se encontró la temporada a eliminar o el id es inválido" };
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return _response;
                }

                _context.Temporada.Remove(temporada);
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

        public async Task<APIResponse> UpdateTemporada(TemporadaDtoUpdate temporadaDto, int id)
        {
            try
            {
                Temporadum temporada = await _context.Temporada.FindAsync(id);

                if (temporada == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string> { "No se encontró la temporada a actualizar" };
                    _response.statusCode = HttpStatusCode.NotFound;
                    return _response;
                }

                _mapper.Map(temporadaDto, temporada);

                _context.Temporada.Update(temporada);
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
        public Task<APIResponse> EndTemporada(TemporadaDtoUpdate temporadaDto, int id)
        {
            throw new NotImplementedException();
        }
    }
}
