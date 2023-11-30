using API_FutbolStats.Models.DtoCreate;
using API_FutbolStats.Models.DtoUpdate;
using API_FutbolStats.Models;

namespace API_FutbolStats.Service.Interfaz
{
    public interface IPartidoJugadoService
    {
        Task<APIResponse> GetPartidosJugador(int idPartido);
        Task<APIResponse> GetPartidosJugadorPorTemporada(int idTemporada, int idJugador);
        Task<APIResponse> GetPartidoJugadorById(int id);
        Task<APIResponse> AddPartidoJugador(PartidoJugadoDtoCreate partidoDto);
        Task<APIResponse> DeletePartidoJugador(int id);
        Task<APIResponse> UpdatePartidoJugador(PartidoJugadoDtoUpdate partidoDto, int id);
    }
}
