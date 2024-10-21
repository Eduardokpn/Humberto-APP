using HumbertoMVC.Models;

public class Parada
{
    // Código identificador da parada
    public int Cp { get; set; }

    // Nome da parada
    public string Np { get; set; }

    // Endereço da parada
    public string Ed { get; set; }

    // Latitude da localização da parada
    public double Py { get; set; }

    // Longitude da localização da parada
    public double Px { get; set; }

    public List<linhasModel> L { get; set; }
}