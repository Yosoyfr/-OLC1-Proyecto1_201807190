using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_Proyecto1_201807190
{
    class Conjunto
    {
        private string nombre;
        private List<char> arrayValue;
        private string auxValor;

        public Conjunto(string nombre, string auxValor)
        {
            this.nombre = nombre;
            this.auxValor = auxValor;
            this.arrayValue = new List<char>();
            this.splitValor();
        }

        public void splitValor() 
        {
            if (auxValor.Length > 1)
            {
                if (auxValor[1] == '~')
                {
                    if (auxValor[0] <= auxValor[2])
                    {
                        for (char c = auxValor[0]; c <= auxValor[2]; c++)
                        {
                            this.arrayValue.Add(c);
                        }
                    }
                    else
                    {
                        for (char c = auxValor[0]; c >= auxValor[2]; c--)
                        {
                            this.arrayValue.Add(c);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < this.auxValor.Length; i++)
                    {
                        if (this.auxValor[i] != ',')
                        {
                            this.arrayValue.Add(this.auxValor[i]);
                        }
                    }
                }
            }
            else 
            {
                this.arrayValue.Add(this.auxValor[0]);
            }
        }

        public string showConjuntos()
        {
            string auxS = this.nombre + " -> ";
            foreach (char aux in arrayValue)
            {
                auxS += aux;
            }
            Console.WriteLine(auxS);
            Console.WriteLine("-------------------------------------------------------------------------------------------");
            return auxS;
        }

        public string Nombre { get => nombre; set => nombre = value; }
        public List<char> ArrayValue { get => arrayValue; set => arrayValue = value; }
        public string AuxValor { get => auxValor; set => auxValor = value; }
    }
}
