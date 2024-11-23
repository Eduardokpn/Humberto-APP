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
            HttpContext.Session.SetString("CoordenadasOrigem", coordenadasJson);
            
            Console.WriteLine("ATUALIZADO: " + DateTime.Now.ToString("HH:mm:ss tt"));
            Console.WriteLine("Latitude: " + coordenadas.Latitude);
            Console.WriteLine("Longitude: " + coordenadas.Longitude);
            var teste = HttpContext.Session.GetString("CoordenadasOrigem");
            // Recupera o cookie
            /*if (HttpContext.Request.Cookies.TryGetValue("CoordenadaasOrigem", out var coordenadasCookies))
            {
                Console.WriteLine($"Valor do cookie: {coordenadasCookies}");
            }*/
            
            
            return Json(new { success = true, message = "Localização recebida com sucesso" });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
}

   


