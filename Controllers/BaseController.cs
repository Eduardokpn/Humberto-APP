using System.Diagnostics;
using Geocoding;
using HumbertoMVC.Controllers.systemControllers;
using HumbertoMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using HumbertoMVC.Models;
using HumbertoMVC.Services;
using LocalizacaoEnderecoRquest = HumbertoMVC.Models.LocalizacaoEnderecoRquest;

namespace HumbertoMVC.Controllers;

[Route("Main/[controller]")]
public class BaseController : Controller
{
    private readonly AdressController _ConvertAndress;
    private readonly RouteController _routeController;
    private readonly GeoCodingService _geoCoding;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BaseController(AdressController convertAndress, RouteController routeController, GeoCodingService geoCoding, IHttpContextAccessor  httpContextAccessor)
    {
        _routeController = routeController ;
        _ConvertAndress = convertAndress ;
        _geoCoding = geoCoding;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    [Route("redirected")]
    public async Task<IActionResult> Redirected(bool redirected)
    {
       
        Console.Error.WriteLine($"Ocorreu um erro ao exibir as rotas verifique as sessoes de rota \n" +
                                $"Metodo ExibirRotas \n" +
                                $"Status Code: {404} \n" + 
                                "Redirecionado para a Home Page");
        return StatusCode(404, new
        {
            
            userMessage = "Ops! tivemos um problema, tente buscar a rota novamente",
            devMessage = "Ocorreu um erro ao buscar as rotas da session RotasOnibus, \n verifique os logs"
        });
        
    }
    
    [HttpPost]
    [Route("iniciarBusca")]
    public async Task<IActionResult> IniciarBusca([FromBody] LocalizacaoEnderecoRquest request)
    {
        // Valida coordenadas origem
        string? sessionCord = _httpContextAccessor.HttpContext?.Session.GetString("CoordenadasOrigem") ?? string.Empty;
        LocationModel.Coordenadas origemCord =
            JsonConvert.DeserializeObject<LocationModel.Coordenadas>(sessionCord);
        
        if (string.IsNullOrEmpty(sessionCord))
        {
            Console.Error.WriteLine($"Erro ao buscar na session os dados da cordenada: {sessionCord} \n" +
                                    $"Session acessada: CoordenadasOrigem \n" +
                                    $"Erro ocorrido no metodo Armazenar Rotas \n");
            return StatusCode(422, new
            {
                userMessage = "Opa! tivemos um problema com sua localização, Verifique se o GPS está ligado e tente novamente.",
                devMessage = "Erro ao buscar na session os dados da cordenada verifique o log sessionCord: " + sessionCord + " \n"
            });
        }
        else if (origemCord.Latitude == null || origemCord.Longitude == null)
        {
            Console.Error.WriteLine($"Erro ao buscar cordenadas de origem, Latitude e Longitude.  \n" +
                                    $"Latitude Origem: {origemCord.Latitude} \n" +
                                    $"Longitude Origem: {origemCord.Longitude} \n" +
                                    $"Erro ocorrido no metodo Armazenar Rotas \n");
            return StatusCode(422, new
            {
                userMessage = "Opa! tivemos um problema com sua localização, Verifique se o GPS está ligado.",
                devMessage = "Faltam cordanadas de origem verifique o log para mais detalhes."
            });
        }
        
        // Recebe endereço destino da funçãoo Buscar.
        string destinyFromBody = request.endereco;
        Adress.GeoResponse destinyConverted = await _geoCoding.ConvertAdress(destinyFromBody); // Latitude e Longitude do destino
        
        // Valida retorno conversão Destino
        if (destinyConverted.Status == "ZERO_RESULTS")
        {
            Console.Error.WriteLine("Endereço de destino está vazio, verifique o processo de converção!");
            return StatusCode(404, new
            {
                userMessage = "Ops! O endereço digitado não foi encontrado!", 
                devMessage = "Endereço de destino está vazio, verifique o processo de converção!"
            });
        }
        else if (destinyConverted.Status != "OK" )
        {
            Console.WriteLine("Convert Adress StatusCode: " + destinyConverted.Status);
            return StatusCode(500, new
            {
                userMessage = "Ops! Ocorreu um erro em nosso sistema. \n Tente novamente mais tarde",
                devMessage = "Convert Adress StatusCode: " + destinyConverted.Status
            });
        }
            
            
            LocationModel.Coordenadas destCoord = new LocationModel.Coordenadas();
            /*  cria o objeto destCoord com os dados retornados da conversão.
                navegando pela lista: result.Geometry.Location, pegando a ultima localização.
                armazenando em: destCoord.Coordenadas.
             */
            foreach (var result in destinyConverted.Results)
            {
                double latitude = result.Geometry.Location.Lat;
                double longitude = result.Geometry.Location.Lng;
                destCoord = new LocationModel.Coordenadas()
                {
                    Latitude = latitude,
                    Longitude = longitude,
                };
            };

            // Armazena as rotas;
            IActionResult ArmazenarRotaResult = await _routeController.ArmazenarRotas(destCoord, origemCord);
            
            //Validar erros
            var statusCode = (ArmazenarRotaResult as ObjectResult)?.StatusCode 
                             ?? (ArmazenarRotaResult as StatusCodeResult)?.StatusCode;
            if (statusCode != 200)
            {
                var objectResult = (ArmazenarRotaResult as ObjectResult)?.Value?.ToString();
                var errorText = JsonConvert.DeserializeObject<errorLogModel>(objectResult);
                
                Console.Error.WriteLine($"Erro ao executar ArmazenarRotas: \n" +
                                        $"StatusCode: {statusCode} \n" +
                                        $"Erro ocorrido no metodo ArmazenarRotas. \n");
                return StatusCode(500, new
                {
                    userMessage = errorText.userMessage,
                    devMessage = errorText.devMessage
                });
            }
            
            //Exibir rotas
            try
            {
                _routeController.ExibirRotas();
           
            return Ok(new { devMessage = "Rotas armazenadas com sucesso!" });
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Ocorreu um erro interno no metodo ExibirRotas \n" +
                                        $"Status Code: {500} \n" +
                                        $"Exception: {e} \n");
                return StatusCode(500, new
                {
                    userMessage = "Ocorreu um erro interno, tente novamente mais tarde.",
                    devMessage = $"Ocorreu um erro interno, verifique os logs,  \n" +
                                 $"Exception: {e}  \n"
                });
            }
        
    }
}