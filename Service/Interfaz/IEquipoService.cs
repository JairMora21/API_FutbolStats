using API_FutbolStats.Models;
using API_FutbolStats.Models.DtoCreate;
using API_FutbolStats.Models.DtoUpdate;

namespace API_FutbolStats.Service.Interfaz
{
    public interface IEquipoService
    {

        Task<APIResponse> GetJugadores();
        Task<APIResponse> GetJugadorById(int id);
        Task<APIResponse> AddJugador(JugadorDtoCreate jugadorDto);
        Task<APIResponse> DeleteJugador(int id);
        Task<APIResponse> UpdateJugador(JugadorDtoUpdate jugadorDto, int id);
        Task<APIResponse> GetStatsJugador(int id, int idTemporada);
        Task<APIResponse> GetStatsEquipoTemporada(int idEquipo, int idTemporada);
        Task<APIResponse> GetStatsEquipoHistorico(int idEquipo);



    }
}
