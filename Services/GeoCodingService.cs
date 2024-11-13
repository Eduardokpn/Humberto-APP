using HumbertoMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HumbertoMVC.Services;

[Route("service/[controller]")]
public class GeoCodingService : Controller
{
    [HttpPost]
    [Route("ConvertAdress")]
    public async Task<Adress.GeoResponse> ConvertAdress(string adress)
    {
        Console.WriteLine(" CONSOLE 2:" + adress);
        HttpClient httpClient = new HttpClient();
        String key = "AIzaSyCb8Qb_FKxRQxVLf4TiInHaZCL9OEVKcUY";
        String uriBase = "https://maps.google.com/maps/api/geocode/";
        String uriComplementar = $"json?address=${adress}&key={key}";
        Console.WriteLine(uriBase + uriComplementar);
        try
        {
            HttpResponseMessage response = await httpClient.PostAsync(uriBase + uriComplementar, null);
            Console.WriteLine(" CONSOLE 3: ENDERECO CONVERTIDO ");
            String responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine(" CONSOLE 4: ENDERECO CONVERTIDO ");
            var deserialize = JsonConvert.DeserializeObject<Adress.GeoResponse>(responseString);
            Console.WriteLine(" CONSOLE 5: ENDERECO CONVERTIDO ");
            if (deserialize == null)
            {
                throw new Exception("Failed to deserialize response");
            }
            
            
            //HttpContext.Session.SetString("adressDestiny", responseString);
            //Console.WriteLine(" TEMPDATA: " +  TempData["adressDestiny"]);
            
            
            //Console.WriteLine("SESSION: " + HttpContext.Session.GetString("adressDestiny"));
            return deserialize;
        }
        catch (Exception e)
        {
            Console.WriteLine("ERRO AO CONVERTER O ENDERECO" + e);
            throw;
        }
    }
    
}