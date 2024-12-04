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
    
    [HttpGet]
    [Route("storeRoutes")]
    public async Task<IActionResult> ArmazenarRotas(LocationModel.Coordenadas destCord, LocationModel.Coordenadas origemCord) 
    {
        try
        {
           
            // VALIDAR DADOS RECEBIDOS
            // Valida objeto origemCord
            if (origemCord.Latitude == null || origemCord.Longitude == null)
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

            // Valida o objeto destCoord
            if (destCord.Latitude == null || destCord.Longitude == null)
            {
                Console.Error.WriteLine($"Erro ao buscar cordenadas de detino, Latitude e Longitude. \n" +
                                        $"Latitude Destino: {destCord.Latitude} \n" +
                                        $"Longitude Destino: {destCord.Longitude} \n" +
                                        $"Erro ocorrido no metodo Armazenar Rotas \n");
                return StatusCode(422, JsonConvert.SerializeObject(new 
                {
                    userMessage = "Opa! tivemos um problema com o endereço digitado",
                    devMessage = "Faltam cordanadas de detino verifique o log para mais detalhes."
                }));
            }

            
            _httpContextAccessor.HttpContext?.Session.Remove("RotasOnibus");
            
            Console.WriteLine("Peso pós remove session RotasOnibus: " + (Encoding.UTF8.GetByteCount((string)JsonConvert.SerializeObject(
                _httpContextAccessor.HttpContext?.Session.GetString("RotasOnibus")))) /  1024);
            
            // Calcular rotas passando origem e destino
            OnibusRotaModel rotasDeserializada = await _routesService.Rotas(origemCord, destCord);
            var RoutesString = JsonConvert.SerializeObject(rotasDeserializada);
            
            if (rotasDeserializada.Routes.Count == 0)
            {
                Console.Error.WriteLine($"Erro ao buscar as rotas, rotasDeserializada, não retornou nenhuma rota \n" +
                                        $"rotasDeserializada: {RoutesString} \n");
                                        
                return StatusCode(400, JsonConvert.SerializeObject(new
                {
                    userMessage = "Lamento, não encontramos uma rota para o endereço informado.",
                    devMessage = "Ocorreu um erro ao calcular a rota, verifique o log para mais detalhes."
                }));
            }
            
            // Para cada Rota atribuir um Codigo Rota 
            foreach (var (route, index) in rotasDeserializada.Routes.Select((route, index) => (route, index)))
            {
                route.Cr = index;
            }

            //Alimentar a sessão com os dados das rotas
            _httpContextAccessor.HttpContext?.Session.SetString("RotasOnibus", RoutesString);
            
            
            return StatusCode(200, "Rotas Armazenadas com sucesso");

        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"Ocorreu um erro interno no metodo ArmazenarRotas \n" +
                                    $"Status Code: {500} \n" +
                                    $"Exception: {e} \n");
            return StatusCode(500, JsonConvert.SerializeObject(new
            {
                userMessage = "Ocorreu um erro interno, tente novamente mais tarde.",
                devMessage = $"Ocorreu um erro interno, verifique os logs,  \n" +
                             $"Exception: {e}  \n"
            }));
        }
    }

    [HttpGet]
    [Route("ShowRoutes")]
    public IActionResult ExibirRotas()
    {
        try
        {
            string? rotasString = _httpContextAccessor.HttpContext?.Session.GetString("RotasOnibus") ?? String.Empty;
            
            if (rotasString == string.Empty || rotasString == "" || rotasString == null)
            {
                Console.Error.WriteLine($"Ocorreu um erro ao exibir as rotas \n" +
                                        $"Metodo ExibirRotas \n" +
                                        $"Status Code: {404} \n" +
                                        $"Dados da session RotasOnibus: {rotasString} \n" +
                                        $"Redirecionando para a Home Page");
                
                return Redirect("/home?redirected=true");
                /*return StatusCode(404, new { 
                    userMessage = $"Ops! tivemos um problema, volte e tente novamente",
                    devMessage = $"Ocorreu um erro ao buscar as rotas da session RotasOnibus, \n verifique os logs"
                });*/
            }
            
           
            
            var rotas = JsonConvert.DeserializeObject<OnibusRotaModel>(rotasString);
           
            return View("Views/Home/Privacy.cshtml", rotas);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"Ocorreu um erro interno no metodo ExibirRotas" +
                                    $"Status Code: {500}" +
                                    $"Exception: {e}");
            return StatusCode(500, new
            {
                userMessage = "Ocorreu um erro interno, tente novamente mais tarde.",
                devMessage = $"Ocorreu um erro interno, verifique os logs, " +
                             $"Exception: {e} "
            });
        }
        
    }
}