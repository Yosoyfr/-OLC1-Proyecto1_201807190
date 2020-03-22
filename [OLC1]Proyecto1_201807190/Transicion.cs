using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_Proyecto1_201807190
{
    class Transicion
    {
        /*
        * Atributos del objeto
        */
        private Estado inicial;
        private Estado final;
        private string simbolo;

        /*
         * Constructor del objeto Transicion que recibe su estado inicial y final
         * y ademas el simbolo por el cual pasar del uno al otro estado
         */
        public Transicion(Estado inicial, Estado final, string simbolo)
        {
            this.Inicial = inicial;
            this.Final = final;
            this.Simbolo = simbolo;
        }

        /*
        * Accesores y modificadores de todos los atributos
        */
        public string Simbolo { get => simbolo; set => simbolo = value; }
        public Estado Inicial { get => inicial; set => inicial = value; }
        public Estado Final { get => final; set => final = value; }

        /*
         * Metodos que muestra como pasa entre el estado
         */
        override
        public string ToString()
        {
            return "(" + Inicial.Id + "-" + Simbolo + "-" + Final.Id + ")";
        }
        public String DOT_String()
        {
            return (this.Inicial + " -> " + this.Final + " [label=\"" + this.Simbolo.Trim(new char[] { '\"' }) + "\"];");
        }
    }
}
