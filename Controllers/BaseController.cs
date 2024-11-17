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

    public BaseController(AdressController convertAndress, RouteController routeController, GeoCodingService geoCoding)
    {
        _routeController = routeController ;
        _ConvertAndress = convertAndress ;
        _geoCoding = geoCoding;
    }
    
    [HttpPost]
    [Route("iniciarBusca")]
    public async Task<IActionResult> IniciarBusca([FromBody] LocalizacaoEnderecoRquest request)
    {
            
            // RECEBER ENDERECO FROM FUNCTION BUSCAR
            string destinyFromBody = request.endereco;
            Adress.GeoResponse enderecoConvert = await _geoCoding.ConvertAdress(destinyFromBody);
            string enderecoConvertString = JsonConvert.SerializeObject(enderecoConvert); 
            
            TempData["adressDestiny"] = enderecoConvertString;
            
            if (enderecoConvertString == null || enderecoConvertString == "")
            {
                Console.WriteLine("Endereço de destino está vazio, verifique o processo de converção! ");
                return BadRequest("Não foi possivel encontrar o destino informado");
            }   
            
            Console.WriteLine("*** Destino armazenado com sucesso ***");
            LocationModel.Coordenadas destCoord = new LocationModel.Coordenadas();
            //Armazenar a rota
            foreach (var result in enderecoConvert.Results)
            {
                double latitude = result.Geometry.Location.Lat;
                double longitude = result.Geometry.Location.Lng;
                destCoord = new LocationModel.Coordenadas()
                {
                    Latitude = latitude,
                    Longitude = longitude,
                };
            };
               
                IActionResult ArmazenarRotaResult = await _routeController.ArmazenarRotaFinal(destCoord);
                string rotaArmazenadaString = JsonConvert.SerializeObject(ArmazenarRotaResult);
                var statusCode = (ArmazenarRotaResult as ObjectResult)?.StatusCode 
                                 ?? (ArmazenarRotaResult as StatusCodeResult)?.StatusCode;
                if (statusCode == 200)
                {
                    Console.WriteLine("Dados de rota armazenados com sucesso - Rota armazenada: " + rotaArmazenadaString);   
                }
                else
                {
                    Console.WriteLine("\n \n Ocorreu um erro ao calcular sua rota StatusCode: " + statusCode + "\n Exception: " + (ArmazenarRotaResult as ObjectResult)?.Value);
                    return BadRequest("Falha ao calcular sua rota!");
                }
                
                
            
            Console.WriteLine("Rota calculada e armazenada com sucesso");
            //Exibir rotas
            try
            {
            _routeController.ExibirRotas();
            Console.WriteLine("Rota Exibir executado");
            return Ok();
            }
            catch (Exception e)
            {
              Console.WriteLine(e);
               return BadRequest("Não foi possivel exibir sua rota!");
            }
        
    }
}