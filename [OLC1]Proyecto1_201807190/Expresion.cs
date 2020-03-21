using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _OLC1_Proyecto1_201807190
{
    class Expresion
    {
        //Atributos del objeto
        string nombre;
        List<string> tokens;
        Automata afn;
        Automata afd;

        public Expresion(string nombre)
        {
            tokens = new List<string>();
            afn = new Automata();
            afd = new Automata();
            this.Nombre = nombre;
        }

        public List<string> Tokens { get => tokens; set => tokens = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public Automata Afn { get => afn; set => afn = value; }
        public Automata Afd { get => afd; set => afd = value; }

        public void convertAFN()
        {
            Queue cola = new Queue();
            List<Estado> cerraduras = cerradura(this.afn.Inicial);
            List<List<Estado>> List_Est = new List<List<Estado>>();
            List_Est.Add(cerraduras);
            cola.Enqueue(cerraduras);

            while (cola.Count > 0)
            {
                Estado newInicial = new Estado(varLetra - 65);
                List<Estado> aux1 = (List<Estado>)cola.Dequeue();
                foreach (string alf in this.afd.Alfabeto)
                {
                    string auxS = "Inicio: " + (char)varLetra + " llegada: ";
                    List<Estado> estados = move(aux1, alf);
                    bool buildfC = false;
                    int auxI = 0;
                    foreach (Estado est in estados)
                    {
                        int c = List_Est.Count;
                        List<Estado> newEsts = cerradura(est);
                        bool perm = true;
                        for (int k = 0; k < c; k++)
                        {
                            if (!compareList(List_Est[k], newEsts))
                            {
                                perm = false;
                                break;
                            }
                            auxI = k;
                        }
                        if (perm)
                        {
                            List_Est.Add(newEsts);
                            cola.Enqueue(newEsts);
                        }
                        auxS += (char)(66 + auxI) + " simbolo: " + alf;
                        buildfC = true;
                    }
                    if (buildfC) 
                    {
                        Estado newFinal = new Estado(auxI + 1);
                        Transicion tran = new Transicion(newInicial, newFinal, alf);
                        newInicial.Transiciones.Add(tran);
                    }
                    Console.WriteLine(auxS);
                }
                this.afd.Estados.Add(newInicial);
                varLetra++;
            }

            this.afd.Tipo = "AFD";
            /*
            foreach (List<Estado> auxList in List_Est)
            {
                string alv = "";
                foreach (Estado aux in auxList)
                {
                    alv += aux.ToString() + ", ";
                }
                Console.WriteLine(alv);
            }
            */
        }

        public bool compareList(List<Estado> list1, List<Estado> list2) 
        {
            List<Estado> result = list2.Except(list1).ToList();
            if (result.Count == 0)
                return false;
            else
                return true;
        }


        public List<Estado> cerradura(Estado estado)
        {
            Stack stackEst = new Stack();
            Estado actual = estado;
            List<Estado> result = new List<Estado>();
            stackEst.Push(actual);

            while (stackEst.Count > 0)
            {
                actual = (Estado)stackEst.Pop();
                foreach (Transicion t in actual.Transiciones)
                {
                    if (t.Simbolo.Equals("Ɛ") && !result.Contains(t.Final))
                    {
                        result.Add(t.Final);
                        stackEst.Push(t.Final);
                    }
                }
            }
            result.Add(estado);

            /*
            string alv = "";
            foreach (Estado est in result)
            {
                alv += est.Id;
            }
            Console.WriteLine(alv);
            */
            return result;
        }

        int varLetra = 65;
        public List<Estado> move(List<Estado> estados, string simbolo)
        {
            List<Estado> alcanzados = new List<Estado>();
            //string auxS = "Inicio: " + (char)varLetra + " llegada: ";
            foreach (Estado est in estados)
            { 
                foreach (Transicion t in est.Transiciones)
                {
                    if (t.Simbolo.Equals(simbolo))
                    {
                        //auxS += (char)(varLetra + 1) + " simbolo: " + simbolo;
                        alcanzados.Add(t.Final);
                    }
                }
            }
            //Console.WriteLine(auxS);
            /*
            string alv = "";
            foreach (Estado est in alcanzados)
            {
                alv += est.Id;
            }
            Console.WriteLine(alv);
            */
            return alcanzados;
        }

        public string getDOTAFN()
        {
            string graph = " digraph G {\n rankdir=LR;\n node[shape = circle];\n ";

            graph += afn.getDOT();

            graph += "}";

            return graph;
        }

        public string getDOTAFD()
        {
            string graph = " digraph G {\n rankdir=LR;\n node[shape = circle];\n ";

            graph += afd.getDOT();

            graph += "}";

            return graph;
        }
    }
}
