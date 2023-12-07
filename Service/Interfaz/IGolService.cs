using API_FutbolStats.Models.DtoCreate;
using API_FutbolStats.Models.DtoUpdate;
using API_FutbolStats.Models;
using API_FutbolStats.Models.Dto;

namespace API_FutbolStats.Service.Interfaz
{
    public interface IGolService
    {
        Task<APIResponse> GetGolesPartido(int idPartido);
        Task<APIResponse> GetGolById(int id);
        Task<APIResponse> AddGol(GolesDtoCreate golesDto);
        Task<APIResponse> DeleteGol(int id);
        Task<APIResponse> UpdateGol(GolesDtoUpdate golesDto, int id);
    }
}
