using HumbertoMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HumbertoMVC.Controllers.spTransController;

[Route("Veicle/[controller]")][ApiController]
public class VeiculoController : Controller
{
    private readonly ApiService _apiService;


    public VeiculoController(ApiService apiService)
    {
        _apiService = apiService; // Instancia o services     
    }

    #region Buscar Previsão de chegada

    [HttpGet]
    [Route("BuscarParadaLinha")]
    public async Task<IActionResult> BuscarPrevisaoPorParadaLinha(int cl, int cp)
    {
        try
        {
            var previsao = await _apiService.GetPrevisaoAsync($"v2.1/Previsao?codigoParada={cp}&codigoLinha={cl}");

            Console.WriteLine(previsao);

            TempData["Previsao"] = JsonConvert.SerializeObject(previsao);

            return Ok("Hora de chega da linha foi buscado e armazenado com Sucesso!!");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, $"Ocorreu um erro interno: \n {e}");
        }
    }

    #endregion

    #region Exibir Previsao de chegada

    [HttpGet]
    [Route("ExibirParadaLinha")]
    public IActionResult ExibirPrevisao()
    {
        try
        {
            var veiculosPrevisaoJson = TempData["Previsao"] as string;

            if (string.IsNullOrEmpty(veiculosPrevisaoJson))
                //return RedirectToAction("BuscarPrevisaoPorParadaLinha");  //TODO: DAR UM JEITO DE CHAMAR O BuscarPrevisaoPorParadaLinha
                return StatusCode(404, "Não foi possivel carregar nenhum onibus,\n" +
                                       " experimente executar novamente 'BuscarPrevisaoPorParadaLinha'  ou verifique o metodo!!");

            var veiculosPrevisao = JsonConvert.DeserializeObject<PrevisaoModel>(veiculosPrevisaoJson);

            TempData["Previsao"] = veiculosPrevisaoJson;

            return View("views/Home/arrivalTest.cshtml", veiculosPrevisao);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "Ocorreu um erro interno ao exibir a previsão!");
        }
    }

    #endregion
}