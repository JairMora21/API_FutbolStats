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
                                                        orderby p.Fecha descending
                                                        select new PartidoDtoStats
                                                        {
                                                            Id = p.Id,
                                                            Equipo = e.Nombre,
                                                            EquipoEscudo = e.Escudo,
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
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Mapeo manual de Result a Partido
                    var result = partidoDto.Result;
                    var partido = new Partido
                    {
                        IdEquipo = result.IdEquipo,
                        IdTemporada = result.IdTemporada,
                        IdTipoPartido = result.IdTipoPartido,
                        IdResultado = result.IdResultado,
                        Fecha = result.Fecha, 
                        NombreRival = result.NombreRival,
                        GolesFavor = result.GolesFavor,
                        GolesContra = result.GolesContra
                    };

                    _context.Partidos.Add(partido);
                    await _context.SaveChangesAsync();

                    foreach (var playerStatDto in partidoDto.PlayerStats)
                    {
                        PlayerStat playerStat = _mapper.Map<PlayerStat>(playerStatDto);

                        PartidoJugadoDtoCreate partidoJugado = new PartidoJugadoDtoCreate
                        {
                            IdJugador = playerStat.Id,
                            IdPartido = partido.Id,
                            IdTemporada = partidoDto.Result.IdTemporada,
                            IdEquipo = partido.IdEquipo
                        };

                        PartidosJugado jugado = _mapper.Map<PartidosJugado>(partidoJugado);
                        _context.PartidosJugados.Add(jugado);


                        if (playerStat.Goles > 0)
                        {
                            GolesDtoCreate golesDtoCreate = new GolesDtoCreate
                            {
                                IdJugador = playerStat.Id,
                                Goles = playerStat.Goles,
                                IdPartido = partido.Id,
                                IdEquipo = partido.IdEquipo,
                                IdTemporada = partidoDto.Result.IdTemporada,
                            };

                            Gole gol = _mapper.Map<Gole>(golesDtoCreate);
                            _context.Goles.Add(gol);
                        }
                        if (playerStat.Amarillas > 0)
                        {
                            TarjetaDtoCreate tarjetaDtoCreate = new TarjetaDtoCreate
                            {
                                IdJugador = playerStat.Id,
                                IdPartido = partido.Id,
                                IdTipoTarjeta = 2,
                                IdEquipo = partido.IdEquipo,
                                IdTemporada = partidoDto.Result.IdTemporada,
                                Tarjetas = playerStat.Amarillas
                            };

                            Tarjetum tarjeta = _mapper.Map<Tarjetum>(tarjetaDtoCreate);
                            _context.Tarjeta.Add(tarjeta);
                        }
                        if (playerStat.Rojas > 0)
                        {
                            TarjetaDtoCreate tarjetaDtoCreate = new TarjetaDtoCreate
                            {
                                IdJugador = playerStat.Id,
                                IdPartido = partido.Id,
                                IdTipoTarjeta = 1,
                                IdEquipo = partido.IdEquipo,
                                IdTemporada = partidoDto.Result.IdTemporada,
                                Tarjetas = playerStat.Rojas
                            };

                            Tarjetum tarjeta = _mapper.Map<Tarjetum>(tarjetaDtoCreate);
                            _context.Tarjeta.Add(tarjeta);
                        }
                    }

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    _response.IsSuccess = true;
                    _response.statusCode = HttpStatusCode.OK;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    _response.IsSuccess = false;
                    _response.statusCode = HttpStatusCode.InternalServerError;
                    _response.ErrorMessages = new List<string> { $"Error:{ex}" };
                }

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


        public async Task<APIResponse> UpdatePartido(PartidoDtoUpdate partidoDto, int idPartido)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Buscar el partido existente
                    var partidoExistente = await _context.Partidos.FindAsync(idPartido);
                    if (partidoExistente == null)
                    {
                        _response.IsSuccess = false;
                        _response.statusCode = HttpStatusCode.NotFound;
                        _response.ErrorMessages = new List<string> { "Partido no encontrado" };
                        return _response;
                    }

                    // Actualizar los datos del partido
                    partidoExistente.IdEquipo = partidoDto.Result.IdEquipo;
                    partidoExistente.IdTemporada = partidoDto.Result.IdTemporada;
                    partidoExistente.IdTipoPartido = partidoDto.Result.IdTipoPartido;
                    partidoExistente.IdResultado = partidoDto.Result.IdResultado;
                    partidoExistente.Fecha = partidoDto.Result.Fecha;
                    partidoExistente.NombreRival = partidoDto.Result.NombreRival;
                    partidoExistente.GolesFavor = partidoDto.Result.GolesFavor;
                    partidoExistente.GolesContra = partidoDto.Result.GolesContra;

                    _context.Partidos.Update(partidoExistente);

                    var participantesExistentes = await _context.PartidosJugados.Where(pj => pj.IdPartido == idPartido).ToListAsync();

                    var jugadoresAEliminar = participantesExistentes.Where(pe => !partidoDto.PlayerStats.Any(ps => ps.Id == pe.IdJugador)).ToList();
                   
                    foreach (var jugadorAEliminar in jugadoresAEliminar)
                    {
                        // Eliminar la participación del jugador
                        _context.PartidosJugados.Remove(jugadorAEliminar);

                        // Eliminar todos los goles del jugador en este partido
                        var golesAEliminar = await _context.Goles.Where(g => g.IdPartido == idPartido && g.IdJugador == jugadorAEliminar.IdJugador).ToListAsync();
                        if (golesAEliminar.Any())
                        {
                            _context.Goles.RemoveRange(golesAEliminar);
                        }

                        // Eliminar todas las tarjetas del jugador en este partido
                        var tarjetasAEliminar = await _context.Tarjeta.Where(t => t.IdPartido == idPartido && t.IdJugador == jugadorAEliminar.IdJugador).ToListAsync();
                        if (tarjetasAEliminar.Any())
                        {
                            _context.Tarjeta.RemoveRange(tarjetasAEliminar);
                        }
                    }

                    //Actualizar o agregar participantes, goles y tarjetas

                    // Actualizar o agregar participantes, goles y tarjetas
                    foreach (var playerStatDto in partidoDto.PlayerStats)
                    {
                        // Actualizar o agregar participantes
                        var participanteExistente = await _context.PartidosJugados
                            .FirstOrDefaultAsync(p => p.IdPartido == idPartido && p.IdJugador == playerStatDto.Id);

                        if (participanteExistente == null)
                        {
                            var nuevoParticipante = new PartidosJugado
                            {
                                IdJugador = playerStatDto.Id,
                                IdPartido = idPartido,
                                IdTemporada = partidoDto.Result.IdTemporada,
                                IdEquipo = partidoExistente.IdEquipo
                            };
                            _context.PartidosJugados.Add(nuevoParticipante);
                        }
                        // Si el participante ya existe, no necesitas hacer nada a menos que tengas más campos en PartidosJugados que necesiten actualización.

                        // Actualizar o agregar goles
                        var golExistente = await _context.Goles
                            .FirstOrDefaultAsync(g => g.IdPartido == idPartido && g.IdJugador == playerStatDto.Id);
                        if (golExistente != null)
                        {
                            if (playerStatDto.Goles > 0)
                            {
                                golExistente.Goles = playerStatDto.Goles;
                                _context.Goles.Update(golExistente);
                            }
                            else
                            {
                                _context.Goles.Remove(golExistente); // Elimina si el número de goles es 0
                            }
                        }
                        else if (playerStatDto.Goles > 0)
                        {
                            var nuevoGol = new Gole
                            {
                                IdJugador = playerStatDto.Id,
                                Goles = playerStatDto.Goles,
                                IdPartido = idPartido,
                                IdEquipo = partidoExistente.IdEquipo,
                                IdTemporada = partidoDto.Result.IdTemporada
                            };
                            _context.Goles.Add(nuevoGol);
                        }

                        // Actualizar o agregar tarjetas amarillas
                        var tarjetaAmarillaExistente = await _context.Tarjeta
                            .FirstOrDefaultAsync(t => t.IdPartido == idPartido && t.IdJugador == playerStatDto.Id && t.IdTipoTarjeta == 2);
                        if (tarjetaAmarillaExistente != null)
                        {
                            if (playerStatDto.Amarillas > 0)
                            {
                                tarjetaAmarillaExistente.Tarjetas = playerStatDto.Amarillas;
                                _context.Tarjeta.Update(tarjetaAmarillaExistente);
                            }
                            else
                            {
                                _context.Tarjeta.Remove(tarjetaAmarillaExistente); // Elimina si el número de tarjetas es 0
                            }
                        }
                        else if (playerStatDto.Amarillas > 0)
                        {
                            var nuevaTarjetaAmarilla = new Tarjetum
                            {
                                IdJugador = playerStatDto.Id,
                                IdPartido = idPartido,
                                IdTipoTarjeta = 2,
                                IdEquipo = partidoExistente.IdEquipo,
                                IdTemporada = partidoDto.Result.IdTemporada,
                                Tarjetas = playerStatDto.Amarillas
                            };
                            _context.Tarjeta.Add(nuevaTarjetaAmarilla);
                        }

                        // Actualizar o agregar tarjetas rojas
                        var tarjetaRojaExistente = await _context.Tarjeta
                            .FirstOrDefaultAsync(t => t.IdPartido == idPartido && t.IdJugador == playerStatDto.Id && t.IdTipoTarjeta == 1);
                        if (tarjetaRojaExistente != null)
                        {
                            if (playerStatDto.Rojas > 0)
                            {
                                tarjetaRojaExistente.Tarjetas = playerStatDto.Rojas;
                                _context.Tarjeta.Update(tarjetaRojaExistente);
                            }
                            else
                            {
                                _context.Tarjeta.Remove(tarjetaRojaExistente); // Elimina si el número de tarjetas es 0
                            }
                        }
                        else if (playerStatDto.Rojas > 0)
                        {
                            var nuevaTarjetaRoja = new Tarjetum
                            {
                                IdJugador = playerStatDto.Id,
                                IdPartido = idPartido,
                                IdTipoTarjeta = 1,
                                IdEquipo = partidoExistente.IdEquipo,
                                IdTemporada = partidoDto.Result.IdTemporada,
                                Tarjetas = playerStatDto.Rojas
                            };
                            _context.Tarjeta.Add(nuevaTarjetaRoja);
                        }
                    }

                    // Guardar cambios y confirmar transacción
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    _response.IsSuccess = true;
                    _response.statusCode = HttpStatusCode.OK;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    _response.IsSuccess = false;
                    _response.statusCode = HttpStatusCode.InternalServerError;
                    _response.ErrorMessages = new List<string> { $"Error:{ex}" };
                }

                return _response;
            }
        }

        public async Task<APIResponse> GetDatosPartido(int id)
        {
            try
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
                                                 where p.Id == id
                                                 select new PartidoDtoStats
                                                 {
                                                     Id = p.Id,
                                                     Equipo = e.Nombre,
                                                     EquipoEscudo = e.Escudo,
                                                     NombreRival = p.NombreRival,
                                                     TipoPartido = tp.TipoPartido1,
                                                     Resultado = rp.Resultado,
                                                     Temporada = t.NombreTemporada,
                                                     Fecha = p.Fecha,
                                                     GolesFavor = p.GolesFavor,
                                                     GolesContra = p.GolesContra
                                                 })
                 .FirstOrDefaultAsync();

                List<GolPartidoDtoStats> Goles = await (from g in _context.Goles
                                                        join j in _context.Jugadors on g.IdJugador equals j.Id
                                                        where g.IdPartido == id
                                                        select new GolPartidoDtoStats
                                                        {
                                                            Id = j.Id,
                                                            Nombre = j.Nombre,
                                                            Cantidad = g.Goles ?? 0
                                                        })
                                                .OrderByDescending(x => x.Cantidad)
                                                .ToListAsync();


                List<TarjetaPartidoDtoStats> tarjetas = await (from g in _context.Tarjeta
                                                               join j in _context.Jugadors on g.IdJugador equals j.Id
                                                               join tt in _context.TipoTarjeta on g.IdTipoTarjeta equals tt.Id
                                                               where g.IdPartido == id
                                                               select new TarjetaPartidoDtoStats
                                                               {
                                                                   Id = j.Id,
                                                                   Nombre = j.Nombre,
                                                                   Tarjeta = tt.Tarjeta,
                                                                   idTipoTarjeta = tt.Id,
                                                                   Cantidad = g.Tarjetas ?? 0 
                                                               })
                                                               .ToListAsync();


                List<PartidoJugadoDtoStats> participaciones = await (from g in _context.PartidosJugados
                                                                     join j in _context.Jugadors on g.IdJugador equals j.Id
                                                                     where g.IdPartido == id
                                                                     group j by new { j.Dorsal, j.Nombre } into grouped
                                                                     select new PartidoJugadoDtoStats
                                                                     {
                                                                         Id = grouped.FirstOrDefault().Id,
                                                                         Nombre = grouped.Key.Nombre,
                                                                         Dorsal = grouped.Key.Dorsal
                                                                     })
                                                           .ToListAsync();

                PartidoCompletoDtoStats result = new PartidoCompletoDtoStats
                {
                    DatosPartido = partido,
                    Goleadores = Goles,
                    Tarjetas = tarjetas,
                    Participantes = participaciones
                };


                _response.Result = result;
                _response.statusCode = HttpStatusCode.OK;

                return _response;

            }
            catch (Exception e)
            {
                _response.ErrorMessages = new List<string> { "Ocurrió un error al procesar la solicitud." };
                _response.IsSuccess = false;
                _response.statusCode = HttpStatusCode.InternalServerError;
                return _response;
            }

        }


    }
}
