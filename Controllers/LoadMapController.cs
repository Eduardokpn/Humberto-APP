using HumbertoMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HumbertoMVC.Controllers;

[Route("[controller]/")]
public class LoadMapController : Controller
{
    private readonly IHttpContextAccessor _httpcontextAcessor;
    

    public LoadMapController(IHttpContextAccessor httpcontextAcessor)
    {
        _httpcontextAcessor = httpcontextAcessor;
        
    }
    [Route("MapLoad")]
    [HttpGet]
    public IActionResult Index()
    {
        return View("/Views/Home/ExibirView.cshtml");
    }

    [Route("GetRouteDetails")]
    [HttpGet]
    public async Task<IActionResult> selectedRoute(int  cr)
    {
        try
        {
            Console.WriteLine("fui solicitado " + cr);
            

            return Json(new { redirectUrl = $"/LoadMap/MapLoad?Cr={cr}" }); //StatusCode(200, JsonConvert.SerializeObject(new { message = $"deu certo {cr}" }));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(404, "Algo deu errado! \n" + e);
        }
       
    }

    [Route(("GetStepsNavigator"))]
    [HttpGet]
    public async Task<OnibusRotaModel.RouteModel> getStepsNavigator([FromQuery] int cr)
    {

        try
        {
            var sessionData = _httpcontextAcessor.HttpContext?.Session.GetString("RotasOnibus");
            
            var allRoutesData =  JsonConvert.DeserializeObject<OnibusRotaModel>(sessionData);
            
            var justRoutes = allRoutesData.Routes;
            var rotaSelecionada = justRoutes?.FirstOrDefault(r => r.Cr == cr);
            
            if (rotaSelecionada == null)
            {
                return null;
            }
            else
            {
                Console.WriteLine("Rota selecionada foi a rota: " + cr );
            }
            
            return rotaSelecionada;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
    
}