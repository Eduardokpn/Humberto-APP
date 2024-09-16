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
        
        [HttpGet]
        [Route("/Home/GetPosicaoTeste")]
        public async Task<IActionResult> GetBusca() 
        {
            try
            {
                // Define o endpoint específico para a requisição 
                var linha = await _apiService.GetLinhasListAsync("v2.1/Linha/Buscar?termosBusca=Tucuruvi", "fd910c4ffe48e5d1c2f50960882fcdc8e2b8f73c4d58defd26e4b2bf9a8cd4e3");
              
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