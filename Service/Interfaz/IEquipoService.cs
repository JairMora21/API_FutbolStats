using API_FutbolStats.Models;

namespace API_FutbolStats.Service.Interfaz
{
    public interface IEquipoService
    {

        Task<APIResponse> GetJugadores();
        Task<APIResponse> GetJugadorById(int id);
        Task<APIResponse> AddJugador(Jugador jugador);
        Task<APIResponse> DeleteJugador(int id);
        Task<APIResponse> UpdateJugador();

    }
}
