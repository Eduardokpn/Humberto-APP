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
        

        public LinhaController()
        {
            _apiService = new ApiService(); // Instancia o services
           
        }
        
        [Route("/Home/GetPosicaoTeste")]
        public async Task<IActionResult> GetBusca() 
        {
            try
            {
                var apiControllerAuth = await new ApiController().PostAuthenticator(); // Autenticar
                if (!(apiControllerAuth is OkObjectResult))
                {
                    Console.WriteLine($"verifique o erro: {apiControllerAuth}");
                    return BadRequest("Falha na autenticação");
                }
        
                Console.WriteLine($" * Retorno Autenticação GetLinha: {apiControllerAuth}");
                
                // Define o endpoint específico para a requisição 
                var jsonResponse = await _apiService.GetAsync("v2.1/Linha/Buscar?termosBusca=800");
                Console.WriteLine($"Resposta da API: {jsonResponse}");
                // Deserializa a resposta JSON
                var linha = JsonConvert.DeserializeObject<linhas>(jsonResponse);

                // Passa os dados para a View
                return View("~/Views/Home/GetPosicaoTeste.cshtml", linha);  // Aqui você passa o modelo para a View retonando para o GetPosicaoTeste
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exceção: {ex}");
                return StatusCode(500, "Ocorreu um erro interno.");
            }
        }
    }
}