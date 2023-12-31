using API_FutbolStats.Models;
using API_FutbolStats.Models.DtoCreate;
using API_FutbolStats.Models.DtoUpdate;

namespace API_FutbolStats.Service.Interfaz
{
    public interface IEquipoService
    {
        //Crud basico
        Task<APIResponse> GetEquipos();
        Task<APIResponse> GetEquipoById(int id);
        Task<APIResponse> GetJugadores();
        Task<APIResponse> GetJugadorById(int id);
        Task<APIResponse> AddJugador(JugadorDtoCreate jugadorDto);
        Task<APIResponse> DeleteJugador(int id);
        Task<APIResponse> UpdateJugador(JugadorDtoUpdate jugadorDto, int id);

        //Obtener datos
        Task<APIResponse> GetStatsJugador(int id, int idTemporada);
        Task<APIResponse> GetStatsEquipo(int idEquipo, int? idTemporada);
        Task<APIResponse> GetStatsGoleador(int idEquipo, int? idTemporada);
        Task<APIResponse> GetStatsPartidos(int idEquipo, int? idTemporada);
        Task<APIResponse> GetStatsAmarillas(int idEquipo, int? idTemporada);
        Task<APIResponse> GetStatsRojas(int idEquipo, int? idTemporada);






    }
}
