using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_Proyecto1_201807190
{
    class Lexema
    {
        private string valor;
        private string evaluador;
        private List<Conjunto> conjuntos;

        public Lexema(string valor, string evaluador)
        {
            this.Valor = valor;
            this.Evaluador = evaluador;
            this.Conjuntos = new List<Conjunto>();
        }

        public void print() 
        {
            Console.WriteLine(this.evaluador + ": " + this.valor);
        }

        public string Valor { get => valor; set => valor = value; }
        public string Evaluador { get => evaluador; set => evaluador = value; }
        public List<Conjunto> Conjuntos { get => conjuntos; set => conjuntos = value; }
    }
}
