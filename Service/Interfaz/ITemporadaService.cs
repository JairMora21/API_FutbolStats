using API_FutbolStats.Models.DtoCreate;
using API_FutbolStats.Models.DtoUpdate;
using API_FutbolStats.Models;

namespace API_FutbolStats.Service.Interfaz
{
    public interface ITemporadaService
    {

        Task<APIResponse> GetTemporadas(int idEquipo);
        Task<APIResponse> GetTemporadaById(int id);
        Task<APIResponse> GetUltimaTemporada(int idEquipo);

        Task<APIResponse> AddTemporada(TemporadaDtoCreate temporadaDto);
        Task<APIResponse> DeleteTemporada(int id);
        Task<APIResponse> UpdateTemporada(TemporadaDtoUpdate temporadaDto, int id);
        Task<APIResponse> EndTemporada(TemporadaDtoUpdate temporadaDto, int id);

    }
}
