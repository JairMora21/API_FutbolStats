using API_FutbolStats.Models;
using API_FutbolStats.Models.Dto;
using API_FutbolStats.Service.Interfaz;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace API_FutbolStats.Service.Implementacion
{
    public class AtributosService : IAtributosService
    {
        private readonly FutbolStatsContext _context;
        private readonly APIResponse _response;
        private readonly IMapper _mapper;


        public AtributosService(FutbolStatsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _response = new APIResponse();
        }

        //Solo se comentara el primer metodo ya que todos hacen la misma funcion, solo con diferentes clases
        public async Task<APIResponse> GetTipoTarjetas()
        {
            //Creamos un try-catch para identificar cualquier problema
            try
            {
                //Obtenemos la lista de nuestra bd
                IEnumerable<TipoTarjetum> tarjetas = await _context.TipoTarjeta.ToListAsync();
                //Si no tiene datos entrara a este if y retornara lo que paso
                if (tarjetas == null)
                {
                    _response.IsSuccess = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    return _response;
                }
                //si si tiene datos, llamaramos a la clase Dto 
                List<TipoTarjetaDto> tarjetasDto = new List<TipoTarjetaDto>();
                //Recorremos la lista de tarjetas
                foreach (var tarjeta in tarjetas)
                {
                    //Mappeamos las tarjetas a nuestra nueva lista dto
                    TipoTarjetaDto tarjetaDto = _mapper.Map<TipoTarjetaDto>(tarjeta);
                    tarjetasDto.Add(tarjetaDto);
                }

                //Devolvemos las tarjetas mapeadas con un status correcto
                _response.Result = tarjetasDto;
                _response.statusCode = HttpStatusCode.OK;

                return _response;
            }
            catch (Exception ex)
            {
                //retornaremos la excepcion junto a otros atributos
                _response.IsSuccess = false;
                _response.statusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string>() { ex.ToString() };

                return _response;
            }
        }

        public async Task<APIResponse> GetClasificacion()
        {
            try
            {
                IEnumerable<ClasificacionTemporadum> clasificacions = await _context.ClasificacionTemporada.ToListAsync();
                if (clasificacions == null)
                {
                    _response.IsSuccess = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    return _response;
                }
                List<ClasificacionTemporadaDto> clasificacionsDto = clasificacions
            .Select(clasificacion => _mapper.Map<ClasificacionTemporadaDto>(clasificacion))
            .ToList();

                _response.Result = clasificacionsDto;
                _response.statusCode = HttpStatusCode.OK;

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.statusCode = HttpStatusCode.InternalServerError;
                _response.Result = $"Error: {ex.Message}";

                return _response;
            }
        }

        public async Task<APIResponse> GetPosicionesJugador()
        {
            try
            {
                IEnumerable<PosicionJugador> posiciones = await _context.PosicionJugadors.ToListAsync();
                if (posiciones == null)
                {
                    _response.IsSuccess = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    return _response;
                }
                List<PosicionJugadorDto> posicionesDto = new List<PosicionJugadorDto>();
                foreach (var posicion in posiciones)
                {
                    // Mapear individualmente cada entidad a su DTO correspondiente
                    PosicionJugadorDto posicionDto = _mapper.Map<PosicionJugadorDto>(posicion);
                    posicionesDto.Add(posicionDto);
                }


                _response.Result = posicionesDto;
                _response.statusCode = HttpStatusCode.OK;

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.statusCode = HttpStatusCode.InternalServerError;
                _response.Result = $"Error: {ex.Message}";

                return _response;
            }
        }

        public async Task<APIResponse> GetTipoPartido()
        {
            try
            {
                IEnumerable<TipoPartido> partidos = await _context.TipoPartidos.ToListAsync();
                if (partidos == null)
                {
                    _response.IsSuccess = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    return _response;
                }
                List<TipoPartidoDto> partidosDto = new List<TipoPartidoDto>();
                foreach (var partido in partidos)
                {
                    // Mapear individualmente cada entidad a su DTO correspondiente
                    TipoPartidoDto posicionDto = _mapper.Map<TipoPartidoDto>(partido);
                    partidosDto.Add(posicionDto);
                }

                _response.Result = partidosDto;
                _response.statusCode = HttpStatusCode.OK;

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.statusCode = HttpStatusCode.InternalServerError;
                _response.Result = $"Error: {ex.Message}";

                return _response;
            }
        }

    }
}
