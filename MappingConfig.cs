using API_FutbolStats.Models;
using API_FutbolStats.Models.Dto;
using API_FutbolStats.Models.DtoCreate;
using API_FutbolStats.Models.DtoUpdate;
using AutoMapper;

namespace API_FutbolStats
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            //Atributos 
            CreateMap<TipoTarjetum, TipoTarjetaDto>().ReverseMap();
            CreateMap<ClasificacionTemporadum, ClasificacionTemporadaDto>().ReverseMap();
            CreateMap<TipoPartido, TipoPartidoDto>().ReverseMap();
            CreateMap<PosicionJugador, PosicionJugadorDto>().ReverseMap();

            //Jugador
            CreateMap<Jugador, JugadorDto>().ReverseMap();
            CreateMap<Jugador, JugadorDtoCreate>().ReverseMap();
            CreateMap<Jugador, JugadorDtoUpdate>().ReverseMap();

            //Temporada
            CreateMap<Temporadum, TemporadaDto>().ReverseMap();
            CreateMap<Temporadum, TemporadaDtoCreate>().ReverseMap();
            CreateMap<Temporadum, TemporadaDtoUpdate>().ReverseMap();

            //Partido
            CreateMap<Partido, PartidoDto>().ReverseMap();
            CreateMap<Partido, PartidoDtoCreate>().ReverseMap();
            CreateMap<Partido, PartidoDtoUpdate>().ReverseMap();

            //PartidoJugado
            CreateMap<PartidosJugado, PartidoJugadoDto>().ReverseMap();
            CreateMap<PartidosJugado, PartidoJugadoDtoCreate>().ReverseMap();
            CreateMap<PartidosJugado, PartidoJugadoDtoUpdate>().ReverseMap();


            //Goles 
            CreateMap<Gole, GolesDto>().ReverseMap();
            CreateMap<Gole, GolesDtoCreate>().ReverseMap();
            CreateMap<Gole, GolesDtoUpdate>().ReverseMap();

            //Tarjetas
            CreateMap<Tarjetum, TarjetaDto>().ReverseMap();
            CreateMap<Tarjetum, TarjetaDtoCreate>().ReverseMap();
            CreateMap<Tarjetum, TarjetaDtoUpdate>().ReverseMap();

        }
    }
}
