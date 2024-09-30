using System.Collections;

namespace HumbertoMVC.Models;

using System;


public class linhasModel
{

        
        public int Cl { get; set; } // Código da linha
        public bool Lc { get; set; } // Se a linha está em circulação
        public string Lt { get; set; } // Letreiro da linha
        public int Sl { get; set; } // Sentido da linha
        public int Tl { get; set; } // Tipo da linha
        public string Tp { get; set; } // Terminal principal
        public string Ts { get; set; } // Terminal secundário
}
