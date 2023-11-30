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
    public class GolService : IGolService
    {
        private readonly FutbolStatsContext _context;
        private readonly APIResponse _response;
        private readonly IMapper _mapper;


        public GolService(FutbolStatsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _response = new APIResponse();
        }
        public async Task<APIResponse> GetGolById(int id)
        {
            try
            {
                Gole gol = await _context.Goles.FindAsync(id);

                if (gol == null || id <= 0)
                {
                    _response.ErrorMessages = new List<string> { "No se encontró el gol" };
                    _response.IsSuccess = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    return _response;
                }
                GolesDto golDto = _mapper.Map<GolesDto>(gol);
                _response.Result = golDto;
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
   

        public async Task<APIResponse> GetGolesJugadorPorTemporada(int idTemporada, int idJugador)
        {
            try
            {
                IEnumerable<Gole> goles = await _context.Goles.Where(x => x.IdTemporada == idTemporada && x.IdJugador == idJugador)
                    .ToListAsync();

                if (goles == null)
                {
                    _response.ErrorMessages = new List<string> { "No se encontraron las temporadas" };
                    _response.IsSuccess = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    return _response;
                }
                List<GolesDto> golesDto = goles
                    .Select(x => _mapper.Map<GolesDto>(x))
                    .ToList();



                _response.Result = golesDto;
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

        public async Task<APIResponse> GetGolesPartido(int idPartido)
        {
            try
            {
                IEnumerable<Gole> goles = await _context.Goles.Where(x => x.IdPartido == idPartido)
                    .ToListAsync();

                if (goles == null)
                {
                    _response.ErrorMessages = new List<string> { "No se encontraron las temporadas" };
                    _response.IsSuccess = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    return _response;
                }
                List<GolesDto> golesDto = goles
                    .Select(x => _mapper.Map<GolesDto>(x))
                    .ToList();

                _response.Result = golesDto;
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
    

        public async Task<APIResponse> AddGol(GolesDtoCreate partidoDto)
        {
            try
            {
                Gole gol= _mapper.Map<Gole>(partidoDto);

                _context.Goles.Add(gol);
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

        public async Task<APIResponse> DeleteGol(int id)
        {
            try
            {
                Gole gol = await _context.Goles.FindAsync(id);

                if (id <= 0 || gol == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string> { "No se encontró el gol a eliminar o el id es inválido" };
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return _response;
                }

                _context.Goles.Remove(gol);
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

        public async Task<APIResponse> UpdateGol(GolesDtoUpdate golDto, int id)
        {
            try
            {
                Gole gol = await _context.Goles.FindAsync(id);

                if (gol == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string> { "No se encontró el gol a actualizar" };
                    _response.statusCode = HttpStatusCode.NotFound;
                    return _response;
                }

                _mapper.Map(golDto, gol);

                _context.Goles.Update(gol);
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
