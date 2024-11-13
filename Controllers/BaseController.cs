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
            Console.WriteLine("\n \n \n \n \n \n ESSA PORRA FUNCIONA?");
            // RECEBER ENDERECO FROM FUNCTION BUSCAR
            
            var enderecoConvert = await _geoCoding.ConvertAdress(request.endereco);
            var enderecoConvertString = JsonConvert.SerializeObject(enderecoConvert); 
            TempData["adressDestiny"] = enderecoConvertString;
            if (enderecoConvertString == null || enderecoConvertString == "")
            {
                Console.WriteLine("Ero ao converter o endereço");
                return BadRequest("ERRO COM O ENDERECO");
            }   
            Console.WriteLine("Destino armazenado com sucesso");
            
            //Armazenar a rota
            try
            {
                var destino = JsonConvert.DeserializeObject<LocationModel.Coordenadas>(enderecoConvertString);
                await _routeController.ArmazenarRotaFinal(destino);
                Console.WriteLine("Rota armazenada");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("Falha ao calcular sua rota!");
            }
            Console.WriteLine("Rota calculada e armazenada com sucesso");
            //Exibir rotas
            //try
            //{
            _routeController.ExibirRotas();
            Console.WriteLine("Rota Exibir executado");
            return Ok();
            //}
            //catch (Exception e)
            //{
              //Console.WriteLine(e);
              // return BadRequest("Não foi possivel exibir sua rota!");
            //}
        
    }
}