namespace HumbertoMVC.Models;

using System;


public class linhas
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
        
        public int cl; // Código identificador da linha. Este é um código identificador único de cada linha do sistema (por sentido de operação)
        public bool lc; // Indica se uma linha opera no modo circular (sem um terminal secundário)
        public int sl; // Informa a primeira parte do letreiro numérico da linha
        public int tl; //  Informa a segunda parte do letreiro numérico da linha, que indica se a linha opera nos modos: BASE(10), ATENDIMENTO(21, 23, 32, 41)
        public string tp;  // Informa o sentido ao qual a linha atende, onde 1 significa Terminal Principal para Terminal Secundário e 2 para Terminal Secundário para Terminal Principal
        public string ts; // Informa o letreiro descritivo da linha no sentido Terminal Principal para Terminal Secundário

}
