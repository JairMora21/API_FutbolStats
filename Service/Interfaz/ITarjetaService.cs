using API_FutbolStats.Models.DtoCreate;
using API_FutbolStats.Models.DtoUpdate;
using API_FutbolStats.Models;

namespace API_FutbolStats.Service.Interfaz
{
    public interface ITarjetaService
    {
        Task<APIResponse> GetTarjetasPartido(int idPartido);
        Task<APIResponse> GetTarjetasById(int id);
        Task<APIResponse> AddTarjetas(TarjetaDtoCreate golesDto);
        Task<APIResponse> DeleteTarjetas(int id);
        Task<APIResponse> UpdateTarjetas(TarjetaDtoUpdate golesDto, int id);
    }
}
