using System.Text;
using HumbertoMVC.Models;
using HumbertoMVC.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HumbertoMVC.Controllers.systemControllers;

[Route("Route/[controller]")]
public class RouteController : Controller
{
    private readonly GeoCodingService _geoCodingService;
    private readonly RoutesService _routesService;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public RouteController(RoutesService routesService, GeoCodingService geoCodingService, IHttpContextAccessor httpContextAccessor)
    {
        _geoCodingService = geoCodingService;
        _routesService = routesService;
        _httpContextAccessor = httpContextAccessor;
    } 
    
    // Recebe o destino em cordenadas e busca as rotas
    [Route("searchRouteByDestiny")]
    [HttpPost]
    public async Task<IActionResult> ArmazenarRotaFinal(LocationModel.Coordenadas destino)
    {
        try
        {

            //var adresJson = TempData["adressDestiny"] as string; //HttpContext.Session.GetString("adressDestiny");
            var adressString = JsonConvert.SerializeObject(destino);
            if (adressString == null || adressString == "")
            {
                return StatusCode(403, "Erro ao buscar cordenadas de destino;");
            }

            var ArmazenarRotaResult = await ArmazenarRotas(destino.Longitude, destino.Latitude);
                
            var statusCode = (ArmazenarRotaResult as ObjectResult)?.StatusCode 
                             ?? (ArmazenarRotaResult as StatusCodeResult)?.StatusCode 
                             ?? 0;
            if (statusCode == 200)
            {
                Console.WriteLine("ROTA ARMAZENADA");
            }
            else
            {
                Console.WriteLine($"\n \n " +
                                  $"** Metodo causador da exception: ArnazenarRotas \n" +
                                  $"** Falha ao armazenar rotas, StatusCode: {statusCode} \n " +
                                  $"Exception: {(ArmazenarRotaResult as ObjectResult)?.Value}" );
                return StatusCode(400, "Falha ao Armazenar as rotas finais");
            }
            
            
            
            return StatusCode(200, "DEU BOM");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
    
    [HttpGet]
    [Route("storeRoutes")]
    public async Task<IActionResult> ArmazenarRotas(double latitude, double longitude) 
    {
        try
        {
            Console.WriteLine("teste breakpoint");
            string? sessionCord = _httpContextAccessor.HttpContext?.Session.GetString("CoordenadasOrigem") ?? string.Empty;
            /*if (HttpContext.Request.Cookies.TryGetValue("CoordenadasOrigem", out var sessionCord))
            {
                Console.WriteLine($"Valor do cookie: {sessionCord}");
            }*/
            
            Console.WriteLine(sessionCord);
            if (string.IsNullOrEmpty(sessionCord))
            {
                // Retorne um erro ou defina um valor padrão
                return StatusCode(422, "Coordenadas não encontradas na sessão. ");
            }
            LocationModel.Coordenadas origemCord =
                JsonConvert.DeserializeObject<LocationModel.Coordenadas>(sessionCord);
            Console.WriteLine("\n \n \n \n \n \n \n VALOR DA deserializado: " + JsonConvert.SerializeObject(origemCord));
            if (origemCord.Latitude == null || origemCord.Longitude == null)
            {
                return StatusCode(422, "Faltam cordanadas de origem verifique se o GPS está ativo e funcionando corretamente");
            }
            LocationModel.Coordenadas destCord = new LocationModel.Coordenadas
            {
                Latitude = latitude,
                Longitude = longitude
            };
            
            
            _httpContextAccessor.HttpContext?.Session.Remove("RotasOnibus");
            Console.WriteLine("Peso pós remove: " + (Encoding.UTF8.GetByteCount((string)JsonConvert.SerializeObject(
                _httpContextAccessor.HttpContext?.Session.GetString("RotasOnibus")))) /  1024);
            
            // Realizando a chamada para obter o objeto deserializado
            OnibusRotaModel rotasDeserializada = await _routesService.Rotas(origemCord, destCord);
            
            Console.WriteLine("\n \n \n \n \n \n \n \n ************************************");
            Console.WriteLine(destCord.Latitude);
            Console.WriteLine(destCord.Longitude);
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
            
            _httpContextAccessor.HttpContext?.Session.SetString("RotasOnibus", RoutesJson);
            
            Console.WriteLine("==============================================================================");
            Console.WriteLine("Origem: ");
            Console.WriteLine(" - Latitude: " + origemCord.Latitude);
            Console.WriteLine(" - Longitude: " + origemCord.Longitude);
            Console.WriteLine("==============================================================================");
            Console.WriteLine("Destino: ");
            Console.WriteLine(" - Latitude: " + destCord.Latitude);
            Console.WriteLine(" - Longitude: " + destCord.Longitude);
            return StatusCode(200, "Rotas Armazenadas com sucesso");

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, $"Ocorreu um erro interno: \n {e}" );
        }
    }

    [HttpGet]
    [Route("ShowRoutes")]
    public IActionResult ExibirRotas()
    {
        Console.WriteLine("Rota do onibus do get :" + _httpContextAccessor.HttpContext?.Session.GetString("RotasOnibus"));
        try
        {
            var jsonString = _httpContextAccessor.HttpContext?.Session.GetString("RotasOnibus");
            
            if (jsonString == string.Empty || jsonString == "" || jsonString == null)
            {
                return StatusCode(404, "Não foi possivel exibir nenhuma rota," +
                                       " certifique-se de que alguma rota foi armazenada");
            }
            
           
            
            var rotas = JsonConvert.DeserializeObject<OnibusRotaModel>(jsonString);
           
            return View("Views/Home/Privacy.cshtml", rotas);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "Ocorreu um erro interno ao exibir as rotas!");
        }
        
    }
    
    
}