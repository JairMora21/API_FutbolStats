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
            //Atributos Mapping
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

        }
    }
}
