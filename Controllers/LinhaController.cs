using HumbertoMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HumbertoMVC.Controllers;
using System;

 
public class LinhaContoller : Controller
{
    private readonly ApiService _apiService;

    public LinhaContoller()
    {
        _apiService = new ApiService(); //alimenta a api base com a url service
    }
    

    public async Task<IActionResult> GetBusca() 
    {
        // Define o endpoint específico para a requisição 
        var jsonResponse = await _apiService.GetAsync("/Linha/Buscar?termosBusca=8000"); //chama a api service passando o endpoint e armazenando na variavel jsonResponse

        // Deserializa a resposta JSON
        linhas Linha = JsonConvert.DeserializeObject<linhas>(jsonResponse); //deserializa a resposta em json e atribui em linha

        // Passa os dados para a View ou para outro processo
        return View(Linha);
    }

}
