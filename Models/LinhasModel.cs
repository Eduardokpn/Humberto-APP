using System.Collections;

namespace HumbertoMVC.Models;

using System;


public class linhasModel
{
        /*
        public int codigoLinha; // Código identificador da linha. Este é um código identificador único de cada linha do sistema (por sentido de operação)
        public bool linhaCircular; // Indica se uma linha opera no modo circular (sem um terminal secundário)
        public string letreiroInicio; // Informa a primeira parte do letreiro numérico da linha
        public string letreiroFinal; //  Informa a segunda parte do letreiro numérico da linha, que indica se a linha opera nos modos: BASE(10), ATENDIMENTO(21, 23, 32, 41)
        public int sentidoLinha;  // Informa o sentido ao qual a linha atende, onde 1 significa Terminal Principal para Terminal Secundário e 2 para Terminal Secundário para Terminal Principal
        public string terminalPrincipal; // Informa o letreiro descritivo da linha no sentido Terminal Principal para Terminal Secundário
        public string terminalSecundario; //  Informa o letreiro descritivo da linha no sentido Terminal Secundário para Terminal Principal
        */
        
        public int Cl { get; set; } // Código da linha
        public bool Lc { get; set; } // Se a linha está em circulação
        public string Lt { get; set; } // Letreiro da linha
        public int Sl { get; set; } // Sentido da linha
        public int Tl { get; set; } // Tipo da linha
        public string Tp { get; set; } // Terminal principal
        public string Ts { get; set; } // Terminal secundário
}
