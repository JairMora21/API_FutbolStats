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

        public async Task<APIResponse> GetPartidos(int idTemporada)
        {
            try
            {
                var existeTemporada = await _context.Temporada.AnyAsync(x => x.Id == idTemporada);

                if (!existeTemporada)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string> { "No se encontró la temporada a actualizar" };
                    _response.statusCode = HttpStatusCode.NotFound;
                    return _response;
                }
                List<PartidoDtoStats> partidos = await (from p in _context.Partidos
                                                        join e in _context.Equipos on p.IdEquipo equals e.Id
                                                        join tp in _context.TipoPartidos on p.IdTipoPartido equals tp.Id
                                                        join rp in _context.ResultadoPartidos on p.IdResultado equals rp.Id
                                                        join t in _context.Temporada on p.IdTemporada equals t.Id
                                                        where p.IdTemporada == idTemporada
                                                        orderby p.Id
                                                        select new PartidoDtoStats
                                                        {
                                                            Id = p.Id,
                                                            Equipo = e.Nombre,
                                                            NombreRival = p.NombreRival,
                                                            TipoPartido = tp.TipoPartido1,
                                                            Resultado = rp.Resultado,
                                                            Temporada = t.NombreTemporada,
                                                            Fecha = p.Fecha,
                                                            GolesFavor = p.GolesFavor,
                                                            GolesContra = p.GolesContra
                                                        })
                                       .ToListAsync();

         

                _response.Result = partidos;
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

        public async Task<APIResponse> GetDatosPartido(int id)
        {
            var existePartido = await _context.Partidos.AnyAsync(x => x.Id == id);

            if (!existePartido)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { "No se encontró la temporada a actualizar" };
                _response.statusCode = HttpStatusCode.NotFound;
                return _response;
            }

            PartidoDtoStats partido = await (from p in _context.Partidos
                                        join e in _context.Equipos on p.IdEquipo equals e.Id
                                        join tp in _context.TipoPartidos on p.IdTipoPartido equals tp.Id
                                        join rp in _context.ResultadoPartidos on p.IdResultado equals rp.Id
                                        join t in _context.Temporada on p.IdTemporada equals t.Id
                                        where p.Id == 1
                                        select new PartidoDtoStats
                                        {
                                            Id = p.Id,
                                            Equipo = e.Nombre,
                                            NombreRival = p.NombreRival,
                                            TipoPartido = tp.TipoPartido1,
                                            Resultado = rp.Resultado,
                                            Temporada = t.NombreTemporada,
                                            Fecha = p.Fecha,
                                            GolesFavor = p.GolesFavor,
                                            GolesContra = p.GolesContra
                                        })
             .FirstOrDefaultAsync();

            _response.Result = partido;
            _response.statusCode = HttpStatusCode.OK;

            return _response;

        }
    }
}
