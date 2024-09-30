using HumbertoMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;


namespace HumbertoMVC.Controllers
{   
    [ApiController]
    public class LinhaController : Controller
    {
        private readonly ApiService _apiService;
        

        public LinhaController(ApiService apiService)
        {
            _apiService = apiService; // Instancia o services
           
           
        }

        #region Pegar Dados (Gets)

        [HttpGet]
        [Route("Controllers/Linhas/BuscarLinha")]
        public async Task<IActionResult> BuscarLinha(string termoBusca) 
        {
            try
            {
                
                // Define o endpoint específico para a requisição 
                var linha = await _apiService.GetLinhasListAsync($"v2.1/Linha/Buscar?termosBusca={termoBusca}");
                Console.WriteLine(linha);
                // Passa os dados para a View
                TempData["Linhas"] = JsonConvert.SerializeObject(linha);

                return Ok("Linhas Buscadas e  armazenadas com Sucesso"); // Aqui você passa o modelo para a View retonando para o GetPosicaoTeste

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exceção: {ex}");
                return StatusCode(500, "Ocorreu um erro interno.");
            }
        }

        #endregion
        

        #region Exibir

        [Route("Controllers/Linhas/ExibirLinha")]
        public IActionResult ExibirLinha(int cl)
        {
            try
            {
                var linhasJson = TempData["Linhas"] as string;

                if (string.IsNullOrEmpty(linhasJson))
                {
                    //return RedirectToAction("GetLinhas");  //TODO: DAR UM JEITO DE CHAMAR O BUSCAR LINHAS PASSANDO UM PARATEMTRO
                    return StatusCode(404, "Não foi possivel carregar nenhuma linha,\n" +
                                           " experimente executar novamente 'GetLinhas'  ou verifique o metodo!!");
                }

                var linhas = JsonConvert.DeserializeObject<List<linhasModel>>(linhasJson);
                var linhasFiltradas = linhas
                    .Where(l => (cl == 0 || l.Cl == cl) /*||  (string.IsNullOrEmpty(lt) || l.Lt == lt)*/)
                    .ToList();

                TempData["Linhas"] = linhasJson; // Repopula o TempData para continuar acessível

                return View("Views/Home/TestesApi.cshtml" ,linhasFiltradas);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "Ocorreu um erro ao exibir as linhas");
            }
            
        }

        #endregion
        
        
    }
    
   
}