using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_Proyecto1_201807190
{
    class Automata
    {
        /*
         * Atributos del objeto
         */
        private Estado inicial;
        private List<Estado> estados_Aceptacion;
        private List<Estado> estados;
        private string tipo;

        /*
         * Constructor vacio del objeto automata
         */
        public Automata()
        {
            this.Estados = new List<Estado>();
            this.Estados_Aceptacion = new List<Estado>();
        }

        public Automata(Estado inicial, List<Estado> estados_Aceptacion, List<Estado> estados)
        {
            this.inicial = inicial;
            this.estados_Aceptacion = estados_Aceptacion;
            this.estados = estados;
        }

        /*
        * Accesores y modificadores de todos los atributos
        */
        public string Tipo { get => tipo; set => tipo = value; }
        public Estado Inicial { get => inicial; set => inicial = value; }
        public List<Estado> Estados_Aceptacion { get => estados_Aceptacion; set => estados_Aceptacion = value; }
        public List<Estado> Estados { get => estados; set => estados = value; }

        override
        public String ToString()
        {
            String res = "";
            res += "-------" + this.tipo + "---------\r\n";
            res += "Estado inicial " + this.inicial + "\r\n";
            res += "Conjunto de transiciones ";
            for (int i = 0; i < this.estados.Count; i++)
            {
                Estado est = estados[i];
                for (int j = 0; j < est.Transiciones.Count; j++)
                {
                    res += est.Transiciones[j] + "-";
                }
            }
            res += "\r\n";
            for (int i = 0; i < this.estados.Count; i++)
            {
                Estado est = estados[i];
                for (int j = 0; j < est.Transiciones.Count; j++)
                {
                    res += est.Transiciones[j].DOT_String();
                }
            }
            res += "\r\n";
            return res;
        }
    }
}
