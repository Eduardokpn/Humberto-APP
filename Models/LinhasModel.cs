namespace HumbertoMVC.Models;

public class linhasModel
{
    public int Cl { get; set; } // Código da linha
    public bool Lc { get; set; } // Se a linha está em circulação
    public string Lt { get; set; } // Letreiro da linha
    public int Sl { get; set; } // Sentido da linha
    public int Tl { get; set; } // Tipo da linha
    public string Tp { get; set; } // Terminal principal
    public string Ts { get; set; } // Terminal secundário


    public string C { get; set; } // Letreiro completo da linha

    public string Lt0 { get; set; } // Letreiro de destino da linha

    public string Lt1 { get; set; } // Letreiro de origem da linha

    public int Qv { get; set; } // Quantidade de veículos localizados

    public List<VeiculoModel> Vs { get; set; } // Relação de veículos localizados
}