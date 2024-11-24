using HumbertoMVC.Models;
using HumbertoMVC.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HumbertoMVC.Controllers.systemControllers;

[Route("Adress/[controller]")]
public class AdressController : Controller
{

    private readonly GeoCodingService _geoCodingService;
    private readonly RouteController _routeController;

    public AdressController(GeoCodingService geoCodingService, RouteController routeController)
    {
        _geoCodingService = geoCodingService;
        _routeController = routeController;
    } 
    
    // Recebe endereco e converte em latitude e longitude para salvar.
    [Route("ConvertAndSaveAdress")] //TODO: RESOLVER ESSE ERRO PARA NÃO USAR O SERVIÇO
    [HttpPost]
    public async Task<IActionResult> ConvertAdressSave(String adress)
    {
        try
        {

            var adressDestiny = _geoCodingService.ConvertAdress(adress);
            var adressDestinyString = JsonConvert.SerializeObject(adressDestiny);
            if (adressDestinyString == null || adressDestinyString == "" )
            {
                return BadRequest("Falha ao converter a rota");
            }
            
            //Salvar coordenadas no Session
            HttpContext.Session.SetString("adressDestiny", adressDestinyString);
           

            return Ok("DEU BOM");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
    
   
}