using API_FutbolStats.Models;
using API_FutbolStats.Models.Dto;
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


        }
    }
}
