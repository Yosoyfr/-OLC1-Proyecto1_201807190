using System;
using System.Collections;
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
        private List<string> alfabeto;

        /*
         * Constructor vacio del objeto automata
         */
        public Automata()
        {
            this.Estados = new List<Estado>();
            this.Estados_Aceptacion = new List<Estado>();
            this.alfabeto = new List<string>();
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
        public List<string> Alfabeto { get => alfabeto; set => alfabeto = value; }

        public string getDOT() {
            string graph = "";
            for (int i = 0; i < this.estados.Count; i++)
            {
                Estado est = estados[i];
                for (int j = 0; j < est.Transiciones.Count; j++)
                {
                    graph += est.Transiciones[j].DOT_String();
                }
            }
            return graph;
        }

        public string getDOTAceptacion()
        {
            string graph = "";
            for (int i = 0; i < this.estados_Aceptacion.Count; i++)
            {
                Estado est = estados_Aceptacion[i];
                if (i + 1 < this.estados_Aceptacion.Count)
                    graph += est + ", ";
                else
                    graph += est;
            }
            return graph;
        }

        public string[,] matriz(int v)
        {
            string[,] est = new string[v, v];

            for (int i = 0; i < this.estados.Count; i++)
            {
                for (int j = 0; j < this.estados[i].Transiciones.Count; j++)
                {
                    Transicion aux = this.estados[i].Transiciones[j];
                    est[aux.Inicial.Id, aux.Final.Id] = aux.Simbolo;
                }
            }
            est[v - 1, v - 1] = "#";
            return est;
        }

        public bool containAceptacion(Estado est) 
        {
            foreach (Estado aux in estados_Aceptacion)
            {
                if(aux.Id == est.Id)
                    return true;
            }
            return false;
        }
        
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
            res += "Estados de aceptacion ";
            res += getDOTAceptacion();
            res += "\r\n";
            return res;
        }
    }
}
