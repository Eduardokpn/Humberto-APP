using System.Text;
using HumbertoMVC.Models;
using HumbertoMVC.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HumbertoMVC.Controllers;


public class RotasExibicaoController : Controller
{

    private readonly RoutesService _routesService;

    public RotasExibicaoController(RoutesService routesService)
    {
        _routesService = routesService;
    }
    
    [HttpGet]
    [Route("Controllers/Rotas/ArmazenarRotas")]
    public async Task<IActionResult> ArmazenarRotas(double latitude, double longitude)
    {
        Console.WriteLine("\n \n \n \n \n SESSIO.GETSTRING: " + HttpContext.Session.GetString("Coordenadas"));
        try
        {
            string sessionCord = HttpContext.Session.GetString("Coordenadas");
            if (string.IsNullOrEmpty(sessionCord))
            {
                // Retorne um erro ou defina um valor padrão
                return BadRequest("Coordenadas não encontradas na sessão.");
            }
            Console.WriteLine("\n \n \n \n \n \n \n VALOR DA SESSION: " + JsonConvert.SerializeObject(sessionCord));
            LocationModel.Coordenadas origemCord =
                JsonConvert.DeserializeObject<LocationModel.Coordenadas>(sessionCord);
            Console.WriteLine("\n \n \n \n \n \n \n VALOR DA deserializado: " + JsonConvert.SerializeObject(origemCord));
            if (origemCord.Latitude == null || origemCord.Longitude == null)
            {
                return StatusCode(422, "Faltam cordanadas de origem verifique se o GPS está ativo");
            }
            LocationModel.Coordenadas destCord = new LocationModel.Coordenadas
            {
                Latitude = latitude,
                Longitude = longitude
            };
            
            
            HttpContext.Session.Remove("RotasOnibus");
            Console.WriteLine("Peso pós remove: " + (Encoding.UTF8.GetByteCount((string)JsonConvert.SerializeObject(
                HttpContext.Session.GetString("RotasOnibus")))) /  1024);
            
            // Realizando a chamada para obter o objeto deserializado
            OnibusRotaModel rotasDeserializada = await _routesService.Rotas(origemCord, destCord);
            if (rotasDeserializada.Routes.Count == 0)
            {
                return StatusCode(400, "Não foi possivel calcular nenhuma rota para as cordenadas informadas");
            }
            
            // Para cada Rota atribuir um Codigo Rota 
            foreach (var (route, index) in rotasDeserializada.Routes.Select((route, index) => (route, index)))
            {
                route.Cr = index;
            }
            
            var RoutesJson = JsonConvert.SerializeObject(rotasDeserializada);
            
            HttpContext.Session.SetString("RotasOnibus", RoutesJson);
            
            Console.WriteLine("==============================================================================");
            Console.WriteLine("Origem: ");
            Console.WriteLine(" - Latitude: " + origemCord.Latitude);
            Console.WriteLine(" - Longitude: " + origemCord.Longitude);
            Console.WriteLine("==============================================================================");
            Console.WriteLine("Destino: ");
            Console.WriteLine(" - Latitude: " + destCord.Latitude);
            Console.WriteLine(" - Longitude: " + destCord.Longitude);
            return Ok("Rotas Armazenadas com sucesso");

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, $"Ocorreu um erro interno: \n {e}" );
        }
    }

    [HttpGet]
    [Route("Controller/Rotas/ExibirRotas")]
    public IActionResult ExibirRotas()
    {
        Console.WriteLine("Rota do onibus do get :" + HttpContext.Session.GetString("RotasOnibus"));
        try
        {
            var jsonString = HttpContext.Session.GetString("RotasOnibus");
            
            if (jsonString == string.Empty || jsonString == "" || jsonString == null)
            {
                return StatusCode(404, "Não foi possivel exibir nenhuma rota," +
                                       " certifique-se de que alguma rota foi armazenada");
            }
            
           
            
            var rotas = JsonConvert.DeserializeObject<OnibusRotaModel>(jsonString);
           
            return View("ViewModel/ExibirRotasViewModel.cshtml", rotas);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "Ocorreu um erro interno ao exibir as rotas!");
        }
        
    }
}