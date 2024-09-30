using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HumbertoMVC.Models;

using System.Net.Http;
using System.Threading.Tasks;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private bool _isAuthenticated = false;
    private string _authToken;

    public ApiService(HttpClient httpClient) // Recebe HttpClient via injeção de dependência
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("http://api.olhovivo.sptrans.com.br");
        Console.WriteLine("----------------------------- EXECUTANDO API --------------------------------");
        
    }
    
    [HttpPost]
    public async Task<bool> PostAuthenticateAsync(string token) //Autentica com o token
    {
        Console.WriteLine("---------------------------- Tentando Autenticar ---------------------------");
        
        try
        {
            var response = await _httpClient.PostAsync($"v2.1/Login/Autenticar?token={token}", null);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            bool isBodyTrue = responseContent.Contains("true", StringComparison.OrdinalIgnoreCase);

            if (isBodyTrue)
            {
                _isAuthenticated = true;
                Console.WriteLine("Retorno da autenticação na ApiServices: " + responseContent);
                return true; // Autenticação bem-sucedida
            }
            else
            {
                // Escreve a mensagem de erro no console
                Console.WriteLine($"*****Erro na autenticação: {response.StatusCode} - {responseContent}");
                Console.WriteLine("-----------------------------//------------------------------------");
                Console.WriteLine("URL AUTENTICATOR É: " + response.RequestMessage.RequestUri);
                Console.WriteLine("-----------------------------//------------------------------------");
                Console.WriteLine($"O retorno do body é: {isBodyTrue}");
                return false; // Falha na autenticação
            }
        }
        catch (Exception ex)
        {
            // Escreve a exceção no console
            Console.WriteLine($"Exceção durante a autenticação: {ex.Message}");
            return false; // Falha na autenticação devido a exceção
        }
    }
    
    
    [HttpGet]
    public async Task<string> GetAsync(string endpoint, string token) //Get Asyncrono
    {
        var response = await _httpClient.GetAsync(endpoint);

        return await response.Content.ReadAsStringAsync();
    }
    
       public async Task<List<linhasModel>> GetLinhasListAsync(string endpoint) //GetLinhaListAsync
        {
            var response = await _httpClient.GetStringAsync(endpoint);
            var linhas = JsonConvert.DeserializeObject<List<linhasModel>>(response);
            return linhas;
        }
    
}
