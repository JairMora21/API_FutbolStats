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

        public async Task<APIResponse> GetEquipos()
        {
            try
            {
                List<EquipoDto> equipos = await _context.Equipos
                 .Select(e => new EquipoDto
                 {
                     Id = e.Id,
                     Nombre = e.Nombre,
                     Lugar = e.Lugar,
                     Escudo = e.Escudo
                 })
                 .ToListAsync();

                if (equipos == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return _response;
                }

                _response.Result = equipos;
                _response.IsSuccess = true;
                _response.statusCode = HttpStatusCode.OK;

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.statusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { $"Error: {ex.Message}" };

                return _response;
            }
        }

        public async Task<APIResponse> GetEquipoById(int id)
        {
            try
            {
                var existeEquipo = await _context.Equipos.AnyAsync(x => x.Id == id);

                if (!existeEquipo)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return _response;
                }

                EquipoDto equipo = await _context.Equipos
                     .Where(j => j.Id == id)
                     .Select(p => new EquipoDto
                     {
                         Id = p.Id,
                         Nombre = p.Nombre,
                         Lugar = p.Lugar,
                         Escudo = p.Escudo

                     })
                      .FirstOrDefaultAsync();


                _response.Result = equipo;
                _response.IsSuccess = true;
                _response.statusCode = HttpStatusCode.OK;

                return _response;
            }
            catch (Exception ex)
            {
                // Manejar la excepción de manera específica, registrarla, etc.
                _response.IsSuccess = false;
                _response.statusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { $"Error: {ex.Message}" };

                return _response;
            }
        }

        public async Task<APIResponse> GetJugadores()
        {
            try
            {
                List<JugadorDto> jugadores = await _context.Jugadors
                    .Include(x => x.IdEquipoNavigation)
                    .Include(x => x.IdPosicionNavigation)
                    .Select(p => new JugadorDto
                    {
                        Id = p.Id,
                        Posicion = p.IdPosicionNavigation.Posicion,
                        Equipo = p.IdEquipoNavigation.Nombre,
                        Nombre = p.Nombre,
                        Apellido = p.Apellido,
                        Img = p.Img,
                        Dorsal = p.Dorsal,
                        Activo = p.Activo

                    })
                    .ToListAsync();

                if (jugadores == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return _response;
                }

                _response.Result = jugadores;
                _response.IsSuccess = true;
                _response.statusCode = HttpStatusCode.OK;

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.statusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { $"Error: {ex.Message}" };

                return _response;
            }

        }
        public async Task<APIResponse> GetJugadorById(int id)
        {
            try
            {
                var existeJugador = await _context.Jugadors.AnyAsync(x => x.Id == id);

                if (!existeJugador)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return _response;
                }

                JugadorDto jugador = await _context.Jugadors
                     .Where(j => j.Id == id)
                     .Include(x => x.IdEquipoNavigation)
                     .Include(x => x.IdPosicionNavigation)
                     .Select(p => new JugadorDto
                     {
                         Id = p.Id,
                         Posicion = p.IdPosicionNavigation.Posicion,
                         Equipo = p.IdEquipoNavigation.Nombre,
                         Nombre = p.Nombre,
                         Apellido = p.Apellido,
                         Img = p.Img,
                         Dorsal = p.Dorsal,
                         Activo = p.Activo
                     })
                      .FirstOrDefaultAsync();


                _response.Result = jugador;
                _response.IsSuccess = true;
                _response.statusCode = HttpStatusCode.OK;

                return _response;
            }
            catch (Exception ex)
            {
                // Manejar la excepción de manera específica, registrarla, etc.
                _response.IsSuccess = false;
                _response.statusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { $"Error: {ex.Message}" };

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
                _response.ErrorMessages = new List<string> { $"Error: {ex.Message}" };

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

        public async Task<APIResponse> GetStatsJugador(int idJugador, int idTemporada)
        {
            JugadorDtoStats stats = new JugadorDtoStats();

            try
            {
                // Validar si el jugador existe
                var jugadorExistente = await _context.Jugadors
                    .AnyAsync(j => j.Id == idJugador);

                // Validar si la temporada existe
                var temporadaExistente = await _context.Temporada
                    .AnyAsync(t => t.Id == idTemporada);

                if (!jugadorExistente || !temporadaExistente)
                {
                    _response.IsSuccess = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { $"Error: no se encontro el jugador o la temporada" };
                    return _response;
                }


                var golesStats = await _context.Goles
                    .Where(g => g.IdJugador == idJugador)
                    .GroupBy(g => g.IdJugador)
                    .Select(g => new
                    {
                        GolesTemp = g.Count(g => g.IdTemporada == idTemporada),
                        GolesTotal = g.Count()
                    })
                    .FirstOrDefaultAsync();

                var partidosJugadosStats = await _context.PartidosJugados
                    .Where(p => p.IdJugador == idJugador)
                    .GroupBy(p => p.IdJugador)
                    .Select(p => new
                    {
                        PartidosJugadosTemp = p.Count(p => p.IdTemporada == idTemporada),
                        PartidosJugadosTotal = p.Count()
                    })
                    .FirstOrDefaultAsync();

                var tarjetasStats = await _context.Tarjeta
                    .Where(t => t.IdJugador == idJugador)
                    .GroupBy(t => t.IdJugador)
                    .Select(t => new
                    {
                        TarjetasRojasTemp = t.Count(t => t.IdTipoTarjeta == 1 && t.IdTemporada == idTemporada),
                        TarjetasAmarillaTemp = t.Count(t => t.IdTipoTarjeta == 2 && t.IdTemporada == idTemporada),
                        TarjetasRojasTotal = t.Count(t => t.IdTipoTarjeta == 1),
                        TarjetasAmarillaTotal = t.Count(t => t.IdTipoTarjeta == 2)
                    })
                    .FirstOrDefaultAsync();

                if (golesStats != null)
                {
                    stats.GolesTemp = golesStats.GolesTemp;
                    stats.GolesTotal = golesStats.GolesTotal;
                }

                if (partidosJugadosStats != null)
                {
                    stats.PartidosJugadosTemp = partidosJugadosStats.PartidosJugadosTemp;
                    stats.PartidosJugadosTotal = partidosJugadosStats.PartidosJugadosTotal;
                }

                if (tarjetasStats != null)
                {
                    stats.AmarillasTemp = tarjetasStats.TarjetasAmarillaTemp;
                    stats.AmarillasTotal = tarjetasStats.TarjetasAmarillaTotal;
                    stats.RojasTemp = tarjetasStats.TarjetasRojasTemp;
                    stats.RojasTotal = tarjetasStats.TarjetasRojasTotal;
                }
                _response.statusCode = HttpStatusCode.OK;
                _response.Result = stats;
                return _response;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.statusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { $"Error: {ex.Message}" };
                return _response;

            }


        }

        private async Task<Dictionary<string, object>> ValidarExistencia(int idEquipo, int? idTemporada)
        {
            Dictionary<string, object> existencia = new()
            {
                ["Success"] = true,
                ["Message"] = "Todas las entidades existen"
            };

            var equipoExistente = await _context.Equipos.AnyAsync(j => j.Id == idEquipo);

            if (!equipoExistente)
            {
                existencia["Success"] = false;
                existencia["Message"] = "Equipo no existe";

                return existencia;
            }

            if (idTemporada.HasValue)
            {
                var temporadaExistente = await _context.Temporada.AnyAsync(t => t.Id == idTemporada.Value);

                if (!temporadaExistente)
                {
                    existencia["Success"] = false;
                    existencia["Message"] = "Temporada no existe";

                    return existencia;
                }
            }
            return existencia;

        }

        public async Task<APIResponse> GetStatsEquipo(int idEquipo, int? idTemporada)
        {
            EquipoStats equipoStats = new EquipoStats();

            try
            {
                Dictionary<string, object> existencia = await ValidarExistencia(idEquipo, idTemporada);

                if (!(bool)existencia["Success"])
                {
                    _response.IsSuccess = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { $"Error: {existencia["Message"]}" };
                    return _response;
                }

                var statsPartidos = await _context.Partidos
                   .Where(x => x.IdEquipo == idEquipo && (!idTemporada.HasValue || x.IdTemporada == idTemporada))
                   .GroupBy(x => x.IdEquipo)
                   .Select(x => new
                   {
                       PartidosJugados = x.Count(),
                       PartidosGanados = x.Count(x => x.IdResultado == 1),
                       PartidosEmpatado = x.Count(x => x.IdResultado == 2),
                       PartidosPerdido = x.Count(x => x.IdResultado == 3),
                       GolesFavor = x.Sum(x => x.GolesFavor),
                       GolesContra = x.Sum(x => x.GolesContra),
                   })
                   .FirstOrDefaultAsync();

                var statsTarjetas = await _context.Tarjeta
                   .Where(x => x.IdEquipo == idEquipo && (!idTemporada.HasValue || x.IdTemporada == idTemporada))
                   .GroupBy(x => x.IdEquipo)
                   .Select(x => new
                   {
                       Rojas = x.Count(x => x.IdTipoTarjeta == 1),
                       Amarillas = x.Count(x => x.IdTipoTarjeta == 2),
                   })
                   .FirstOrDefaultAsync();

                if (statsPartidos != null)
                {
                    equipoStats.PartidosJugados = statsPartidos.PartidosJugados;
                    equipoStats.PartidosGanados = statsPartidos.PartidosGanados;
                    equipoStats.PartidosEmpatados = statsPartidos.PartidosEmpatado;
                    equipoStats.PartidosPerdidos = statsPartidos.PartidosPerdido;
                    equipoStats.GolesAnotados = statsPartidos.GolesFavor;
                    equipoStats.GolesRecibidos = statsPartidos.GolesContra;
                    equipoStats.GolesDiferencia = statsPartidos.GolesFavor - statsPartidos.GolesContra;
                    int puntos = (statsPartidos.PartidosGanados * 3) + (statsPartidos.PartidosEmpatado);
                    equipoStats.Puntos = puntos;


                }

                if (statsTarjetas != null)
                {
                    equipoStats.Amarillas = statsTarjetas.Amarillas;
                    equipoStats.Rojas = statsTarjetas.Rojas;
                }

                _response.Result = equipoStats;
                _response.statusCode = HttpStatusCode.OK;
                return _response;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.statusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { $"Error: {ex.Message}" };
                return _response;
            }

        }

        public async Task<APIResponse> GetStatsGoleador(int idEquipo, int? idTemporada, int? limite)
        {
            try
            {
                Dictionary<string, object> existencia = await ValidarExistencia(idEquipo, idTemporada);

                if (!(bool)existencia["Success"])
                {
                    _response.IsSuccess = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { $"Error: {existencia["Message"]}" };
                    return _response;
                }

                //Se ejecuta el query para obtener el top de goleadores
                IQueryable<TopListasDtoStats> query =
           from g in _context.Goles
           join j in _context.Jugadors on g.IdJugador equals j.Id
           where g.IdEquipo == idEquipo &&
                 (!idTemporada.HasValue || g.IdTemporada == idTemporada)
           group g by new { j.Id, j.Nombre } into grouped
           select new TopListasDtoStats
           {
               Id = grouped.Key.Id,
               Nombre = grouped.Key.Nombre,
               Cantidad = grouped.Count()
           };
                //ordenamos de mayor goles a menor 
                query = query.OrderByDescending(x => x.Cantidad);
                //Si se selecciono un limite se debe recortan los registros correponsiente al limite 
                if (limite.HasValue)
                {
                    query = query.Take(limite.Value);
                }
                //Traemos la lista de goleadores
                List<TopListasDtoStats> result = await query.ToListAsync();

                _response.Result = result;
                _response.statusCode = HttpStatusCode.OK;

                return _response;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.statusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { $"Error: {ex.Message}" };
                return _response;
            }
        }

        public async Task<APIResponse> GetStatsPartidos(int idEquipo, int? idTemporada, int? limite)
        {
            try
            {
                Dictionary<string, object> existencia = await ValidarExistencia(idEquipo, idTemporada);

                if (!(bool)existencia["Success"])
                {
                    _response.IsSuccess = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { $"Error: {existencia["Message"]}" };
                    return _response;
                }

                //Se ejecuta el query para obtener el top de goleadores
                IQueryable<TopListasDtoStats> query =
      from p in _context.PartidosJugados
      join j in _context.Jugadors on p.IdJugador equals j.Id
      where p.IdEquipo == idEquipo &&
            (!idTemporada.HasValue || p.IdTemporada == idTemporada)
      group p by new { j.Id, j.Nombre } into grouped
      select new TopListasDtoStats
      {
          Id = grouped.Key.Id,
          Nombre = grouped.Key.Nombre,
          Cantidad = grouped.Count()
      };
                //ordenamos de mayor partidos jugados a menor 
                query = query.OrderByDescending(x => x.Cantidad);

                //Si se selecciono un limite se debe recortan los registros correponsiente al limite 
                if (limite.HasValue)
                {
                    query = query.Take(limite.Value);
                }
                //Creamos la lista 
                List<TopListasDtoStats> result = await query.ToListAsync();

                _response.Result = result;
                _response.statusCode = HttpStatusCode.OK;

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.statusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { $"Error: {ex.Message}" };
                return _response;

            }
        }

        public async Task<APIResponse> GetStatsAmarillas(int idEquipo, int? idTemporada)
        {

            try
            {
                Dictionary<string, object> existencia = await ValidarExistencia(idEquipo, idTemporada);

                if (!(bool)existencia["Success"])
                {
                    _response.IsSuccess = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { $"Error: {existencia["Message"]}" };
                    return _response;
                }

                List<TopListasDtoStats> result = await (from t in _context.Tarjeta
                                                        join j in _context.Jugadors on t.IdJugador equals j.Id
                                                        where t.IdEquipo == idEquipo
                                                        && t.IdTipoTarjeta == (int)TipoTarjeta.Amarilla
                                                        && (!idTemporada.HasValue || t.IdTemporada == idTemporada)
                                                        group t by new { j.Id, j.Nombre } into grouped
                                                        select new TopListasDtoStats
                                                        {
                                                            Id = grouped.Key.Id,
                                                            Nombre = grouped.Key.Nombre,
                                                            Cantidad = grouped.Count()
                                                        })
                             .OrderByDescending(x => x.Cantidad)
                             .ToListAsync();

                _response.Result = result;
                _response.statusCode = HttpStatusCode.OK;

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.statusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { $"Error: {ex.Message}" };
                return _response;

            }
        }

        public async Task<APIResponse> GetStatsRojas(int idEquipo, int? idTemporada)
        {
            try
            {
                Dictionary<string, object> existencia = await ValidarExistencia(idEquipo, idTemporada);

                if (!(bool)existencia["Success"])
                {
                    _response.IsSuccess = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { $"Error: {existencia["Message"]}" };
                    return _response;
                }

                List<TopListasDtoStats> result = await (from t in _context.Tarjeta
                                                        join j in _context.Jugadors on t.IdJugador equals j.Id
                                                        where t.IdEquipo == idEquipo
                                                        && t.IdTipoTarjeta == (int)TipoTarjeta.Roja
                                                        && (!idTemporada.HasValue || t.IdTemporada == idTemporada)
                                                        group t by new { j.Id, j.Nombre } into grouped
                                                        select new TopListasDtoStats
                                                        {
                                                            Id = grouped.Key.Id,
                                                            Nombre = grouped.Key.Nombre,
                                                            Cantidad = grouped.Count()
                                                        })
                         .OrderByDescending(x => x.Cantidad)
                         .ToListAsync();

                _response.Result = result;
                _response.statusCode = HttpStatusCode.OK;

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.statusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { $"Error: {ex.Message}" };
                return _response;

            }
        }

        public async Task<APIResponse> GetStatsTarjetas(int idEquipo, int? idTemporada, int? limite)
        {
            try
            {
                Dictionary<string, object> existencia = await ValidarExistencia(idEquipo, idTemporada);

                if (!(bool)existencia["Success"])
                {
                    _response.IsSuccess = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { $"Error: {existencia["Message"]}" };
                    return _response;
                }

                IQueryable<TopTarjetasDtoStats> query = (from t in _context.Tarjeta
                                                         join j in _context.Jugadors on t.IdJugador equals j.Id
                                                         where t.IdEquipo == idEquipo &&
                                                               (!idTemporada.HasValue || t.IdTemporada == idTemporada)
                                                         group t by new { j.Id, j.Nombre } into grouped
                                                         select new TopTarjetasDtoStats
                                                         {
                                                             Id = grouped.Key.Id,
                                                             Nombre = grouped.Key.Nombre,
                                                             Amarillas = grouped.Count(x => x.IdTipoTarjeta == 2),
                                                             Rojas = grouped.Count(x => x.IdTipoTarjeta == 1),
                                                             Total = grouped.Count(x => x.IdTipoTarjeta == 1 || x.IdTipoTarjeta == 2)

                                                         });


                query = query.OrderByDescending(x => x.Total);

                //Si se selecciono un limite se debe recortan los registros correponsiente al limite 
                if (limite.HasValue)
                {
                    query = query.Take(limite.Value);
                }
                //Creamos la lista 
                List<TopTarjetasDtoStats> result = await query.ToListAsync();

                _response.Result = result;
                _response.statusCode = HttpStatusCode.OK;


                _response.Result = result;
                _response.statusCode = HttpStatusCode.OK;

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.statusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { $"Error: {ex.Message}" };
                return _response;

            }
        }

        public enum TipoTarjeta
        {
            Roja = 1,
            Amarilla = 2,
        }
    }
}
