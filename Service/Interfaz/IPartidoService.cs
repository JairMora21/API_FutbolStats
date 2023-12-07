using API_FutbolStats.Models.DtoCreate;
using API_FutbolStats.Models.DtoUpdate;
using API_FutbolStats.Models;

namespace API_FutbolStats.Service.Interfaz
{
    public interface IPartidoService
    {
        Task<APIResponse> GetPartidos(int idTemporada);
        Task<APIResponse> GetPartidoById(int id);
        Task<APIResponse> GetDatosPartido(int id);
        Task<APIResponse> AddPartido(PartidoDtoCreate partidoDto);
        Task<APIResponse> DeletePartido(int id);
        Task<APIResponse> UpdatePartido(PartidoDtoUpdate partidoDto, int id);
    }
}
