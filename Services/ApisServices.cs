namespace HumbertoMVC.Models;

using System.Net.Http;
using System.Threading.Tasks;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private bool _isAuthenticated = false; // Verifica se está autenticado com o Token  

    public ApiService() //recebe o url base da api
    {
        Console.WriteLine("----------------------------- EXECUTANDO API --------------------------------");
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://api.olhovivo.sptrans.com.br/v2.1")
        };
    }

    public async Task<bool> PostAuthenticateAsync(string token) //Autentica com o token
    {
        var requestUrl = _httpClient.BaseAddress + "/Login/Autenticar?token=" + token;
        Console.WriteLine($"Requisição URL: {requestUrl}");
        
        

        try
        {
            string url = await _httpClient.GetStringAsync("/Login/Autenticar?token={token}");
            var response = await _httpClient.PostAsync("/Login/Autenticar?token=" + token, null);
            
            if (response.IsSuccessStatusCode)
            {
                _isAuthenticated = true;
                return true; // Autenticação bem-sucedida
            }
            else
            {
                // Escreve a mensagem de erro no console
                var errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"*****Erro na autenticação: {response.StatusCode} - {errorMessage}");
                Console.WriteLine("-----------------------------//------------------------------------");
                Console.WriteLine("URL AUTENTICATOR É: " + response);
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
    
    public async Task<string> GetAsync(string endpoint) //recebe o endpoint da api para getAsync
    {
        
        var response = await _httpClient.GetAsync(endpoint);
        response.EnsureSuccessStatusCode(); // Garante que o status da resposta foi 200-299

        return await response.Content.ReadAsStringAsync();
    }
}
