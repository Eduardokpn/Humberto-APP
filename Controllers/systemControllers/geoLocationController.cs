using HumbertoMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HumbertoMVC.Controllers.systemControllers;

[Route("Location/[controller]")]
public class geoLocationController : Controller
{
    
    [Route("viewCurrentLocation")]
    [HttpGet]
    public IActionResult Index()
    {
        return View("ViewModel/calcularRota.cshtml");
    }
    
    [Route("getCurrentLocation")]
    [HttpPost]
    public IActionResult SaveLocation([FromBody] LocationModel.Coordenadas coordenadas)
    {
        try
        {
            // Use a latitude e longitude conforme necessário
            var coordenadasJson = JsonConvert.SerializeObject(coordenadas);

            //Salvar coordenadas no Session
            HttpContext.Session.SetString("Coordenadas", coordenadasJson);
            
            Console.WriteLine(" \n \n \n DADOS DA SESSAO ANTES DE ENVIAR: " +  HttpContext.Session.GetString("Coordenadas"));

            Console.WriteLine("ATUALIZADO: " + DateTime.Now.ToString("HH:mm:ss tt"));
            Console.WriteLine("Latitude: " + coordenadas.Latitude);
            Console.WriteLine("Longitude: " + coordenadas.Longitude);

            return Json(new { success = true, message = "Localização recebida com sucesso" });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
}

   


