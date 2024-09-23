namespace HumbertoMVC.Controllers;

using HumbertoMVC.Models;
using Microsoft.AspNetCore.Mvc;

public class ApiController : Controller
{
    private readonly ApiService _apiService;

    public ApiController(ApiService apiService)
    {
        _apiService = apiService; // Instancia uma nova api service
    }

    [HttpGet]
    [Route("/Home/Autenticate")]
    public async Task<IActionResult> PostAuthenticator() 
    {   
        // Autentica na API usando o token fornecido
        string token = "fd910c4ffe48e5d1c2f50960882fcdc8e2b8f73c4d58defd26e4b2bf9a8cd4e3"; // Substitua pelo seu token
        bool authenticated = await _apiService.PostAuthenticateAsync(token);
        
        if (authenticated)
        {
            Console.WriteLine($"  * Autenticado Com Sucesso na Api Controller - {authenticated}");
            return Ok("Autenticado com sucesso!");
        }
        else
        {
            return Unauthorized("  * Falha na autenticação. * ");
        }
    }
    
}