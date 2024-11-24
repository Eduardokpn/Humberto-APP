using System.Text.Json.Serialization;

namespace HumbertoMVC.Models;

public class errorLogModel
{
    public string userMessage { get; set; }
    public string devMessage { get; set; }
}