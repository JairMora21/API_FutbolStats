using API_FutbolStats.Models;

namespace API_FutbolStats.Service.Interfaz
{
    public interface IAtributosService
    {
        Task<APIResponse> GetTipoTarjetas();
        Task<APIResponse> GetPosicionesJugador();
        Task<APIResponse> GetTipoPartido();
        Task<APIResponse> GetClasificacion();
    }
}
