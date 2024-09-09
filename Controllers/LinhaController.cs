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
        
        [Route("/Home/GetPosicaoTeste")]
        public async Task<IActionResult> GetBusca() 
        {
            try
            {
               // Console.WriteLine($" * Retorno Autenticação GetLinha: {apiControllerAuth}");
                
                // Define o endpoint específico para a requisição 
                var jsonResponse = await _apiService.GetAsync("v2.1/Linha/Buscar?termosBusca=Lapa", "fd910c4ffe48e5d1c2f50960882fcdc8e2b8f73c4d58defd26e4b2bf9a8cd4e3");
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