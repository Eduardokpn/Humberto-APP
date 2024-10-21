namespace HumbertoMVC.Models;

public class VeiculoModel
{
    // Letreiro completo da linha
    public string C { get; set; }

    // Código identificador da linha
    public int C1 { get; set; }

    // Sentido de operação (1 ou 2)
    public int S1 { get; set; }

    // Letreiro de destino da linha
    public string L10 { get; set; }

    // Letreiro de origem da linha
    public string L11 { get; set; }

    // Quantidade de veículos localizados
    public int Qv { get; set; }

    // Prefixo do veículo
    public int P { get; set; }

    // Horário previsto para chegada do veículo no ponto de parada relacionado
    public string T { get; set; }

    // Acessível para pessoas com deficiência
    public bool A { get; set; }

    // Horário universal (UTC) da localização
    public string Ta { get; set; }

    // Latitude da localização do veículo
    public double Py { get; set; }

    // Longitude da localização do veículo
    public double Px { get; set; }
}