using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_Proyecto1_201807190
{
    class Tabla_Transiciones
    {
        private Estado estado;
        private List<Estado> alcanzados;
        private List<Estado> cerraduras;

        public Tabla_Transiciones(Estado estado)
        {
            this.Estado = estado;
            this.Alcanzados = new List<Estado>();
            this.Cerraduras = new List<Estado>();
        }

        public Estado Estado { get => estado; set => estado = value; }
        public List<Estado> Alcanzados { get => alcanzados; set => alcanzados = value; }
        public List<Estado> Cerraduras { get => cerraduras; set => cerraduras = value; }

        public string getDOT() 
        {
            string aux = "";
            string aux1 = "";
            for (int i = 0; i < this.cerraduras.Count; i++)
            {
                Estado est = cerraduras[i];
                if (i + 1 < this.cerraduras.Count)
                    aux1 += est + ", ";
                else
                    aux1 += est;
            }
            aux += "<tr>\n\t\t\t<td><b>" + aux1 + "</b></td>\n";
            aux += "\t\t\t<td><b>" + this.estado.ToString() + "</b></td>\n";
            foreach (Estado est in alcanzados)
            {
                if (est.Id != -1)
                    aux += "\t\t\t<td><b>" + est.ToString() + "</b></td>\n";
                else
                    aux += "\t\t\t<td>-</td>\n";
            }
            aux += "\t\t</tr>\n";
            return aux;
        }

        override
        public string ToString()
        {
            string aux = "  " + this.estado.ToString() + "    ---  ";
            foreach (Estado est in alcanzados)
            {
                if (est.Id != -1)
                    aux += est.ToString() + "  ---  ";
                else
                    aux += "/" + "  ---  ";
            }
            return aux;
        }
    }
}
