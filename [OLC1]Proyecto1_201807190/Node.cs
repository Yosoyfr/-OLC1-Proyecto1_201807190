    using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_Proyecto1_201807190
{
    class Node
    {
        public string data;
        public Node right = null;
        public Node left = null;
        private Automata afn;

        public Node()
        {
        }

        //Nodo padre con 2 hijos
        public Node(string data, Node right, Node left)
        {
            this.data = data;
            this.right = right;
            this.left = left;
        }

        //Nodo padre con 1 hijo
        public Node(string data, Node right)
        {
            this.data = data;
            this.right = right;
            this.left = null;
        }

        //Nodo unitario
        public Node(string data)
        {
            this.data = data;
            this.right = null;
            this.left = null;
        }
        public void processNode(Stack stackResults)
        {
            if (left != null)
            {
                left.processNode(stackResults);
            }

            if (right != null)
            {
                right.processNode(stackResults);

            }

            string result = "";

            switch (data)
            {
                case Operador.Concatenar:
                    result += (string)stackResults.Pop() + "." + (string)stackResults.Pop();
                    break;
                case Operador.Disyuncion:
                    result += (string)stackResults.Pop() + "|" + (string)stackResults.Pop();
                    break;
                case Operador.Cerradura_Positiva:
                    result += ")" + (string)stackResults.Pop() + "(+";
                    break;
                case Operador.Kleene:
                    result += ")" + (string)stackResults.Pop() + "(*";
                    break;
                case Operador.Interrogacion:
                    result += ")" + (string)stackResults.Pop() + "(?";
                    break;
                default:
                    result += data;
                    break;
            }
            stackResults.Push(result);
        }

        //Proceso de thompson
        
        public void buildAFN(Stack stackAFN)
        {
            if (left != null)
            {
                left.buildAFN(stackAFN);
            }

            if (right != null)
            {
                right.buildAFN(stackAFN);

            }
            switch (data)
            {
                case Operador.Concatenar:
                    Automata C1 = (Automata)stackAFN.Pop();
                    Automata C2 = (Automata)stackAFN.Pop();
                    Automata concat_Result = concatenacion(C1, C2);

                    stackAFN.Push(concat_Result);
                    this.afn = concat_Result;
                    break;

                case Operador.Disyuncion:
                    Automata D1 = (Automata)stackAFN.Pop();
                    Automata D2 = (Automata)stackAFN.Pop();
                    Automata disyun_Result = disyuncion(D1, D2);
                    stackAFN.Push(disyun_Result);
                    this.afn = disyun_Result;
                    break;

                case Operador.Kleene:
                    Automata K = cerraduraKleene((Automata)stackAFN.Pop());
                    stackAFN.Push(K);
                    this.afn = K;
                    break;

                case Operador.Interrogacion:
                    Automata auxI = AFNSimple("Ɛ");
                    stackAFN.Push(auxI);
                    Automata I1 = (Automata)stackAFN.Pop();
                    Automata I2 = (Automata)stackAFN.Pop();
                    Automata int_Result = disyuncion(I1, I2);
                    stackAFN.Push(int_Result);
                    this.afn = int_Result;
                    break;

                default:
                    Automata simple = AFNSimple(data);
                    stackAFN.Push(simple);
                    this.afn = simple;
                    break;
            }
            this.afn.Tipo = "AFN";
        }


        public Automata duplicateAFN(List<Estado> aceptacion, List<Estado> estados_)
        {
            Automata automataFN = new Automata();

            for (int i = 0; i < estados_.Count; i++)
            {
                Estado newEstado = new Estado(int.Parse(estados_[i].ToString()));
                for (int j = 0; j < estados_[i].Transiciones.Count; j++)
                {
                    newEstado = new Estado(int.Parse(estados_[i].Transiciones[j].Inicial.ToString()));
                    Estado newFinal = new Estado(int.Parse(estados_[i].Transiciones[j].Final.ToString()));
                    Transicion tran = new Transicion(newEstado, newFinal, estados_[i].Transiciones[j].Simbolo.ToString());
                    newEstado.Transiciones.Add(tran);
                }
                automataFN.Estados.Add(newEstado);
            }

            automataFN.Inicial = automataFN.Estados[0];

            for (int i = 0; i < aceptacion.Count; i++)
            {
                Estado newEstado = new Estado(int.Parse(aceptacion[i].ToString()));

                for (int j = 0; j < aceptacion[i].Transiciones.Count; j++)
                {
                    newEstado = new Estado(int.Parse(aceptacion[i].Transiciones[j].Inicial.ToString()));
                    Estado newFinal = new Estado(int.Parse(aceptacion[i].Transiciones[j].Final.ToString()));
                    Transicion tran = new Transicion(newEstado, newFinal, aceptacion[i].Transiciones[j].Simbolo.ToString());
                    newEstado.Transiciones.Add(tran);
                }
                automataFN.Estados_Aceptacion.Add(newEstado);
            }

            return automataFN;
        }

        /*
         * Metodo para estados simples del automata que no han recibido alguna orden de algun simbolo
         */
        public Automata AFNSimple(string simbolo)
        {
            Automata automataFN = new Automata();

            Estado inicial = new Estado(0);
            Estado aceptacion = new Estado(1);

            Transicion tran = new Transicion(inicial, aceptacion, simbolo);
            inicial.Transiciones.Add(tran);

            automataFN.Estados.Add(inicial);
            automataFN.Estados.Add(aceptacion);

            automataFN.Inicial = inicial;
            automataFN.Estados_Aceptacion.Add(aceptacion);
            return automataFN;
        }

        /*
         * Metodo para concatenar estados de automatas
         */
        public Automata concatenacion(Automata AFN1, Automata AFN2)
        {
            Automata afn_concat = new Automata();

            int i = 0;
            for (i = 0; i < AFN2.Estados.Count; i++)
            {
                Estado temp = (Estado)AFN2.Estados[i];
                temp.Id = i;

                if (i == 0)
                {
                    afn_concat.Inicial = temp;
                }

                if (i == AFN2.Estados.Count - 1)
                {
                    for (int j = 0; j < AFN2.Estados_Aceptacion.Count; j++)
                    {
                        foreach (Transicion t in AFN1.Inicial.Transiciones)
                        {
                            temp.Transiciones.Add(new Transicion((Estado)AFN2.Estados_Aceptacion[j], t.Final, t.Simbolo));
                        }
                        //temp.Transiciones.Add(new Transicion((Estado)AFN2.Estados_Aceptacion[j], AFN1.Inicial, "ε"));
                    }
                }
                afn_concat.Estados.Add(temp);
            }
            for (int j = 1; j < AFN1.Estados.Count; j++)
            {
                Estado temp = (Estado)AFN1.Estados[j];
                temp.Id = i;
                if (j == AFN1.Estados.Count - 1)
                {
                    afn_concat.Estados_Aceptacion.Add(temp);
                }
                afn_concat.Estados.Add(temp);
                i++;
            }
            
            return afn_concat;
        }

        public Automata disyuncion(Automata AFN2, Automata AFN1)
        {
            Automata afn_disyun = new Automata();

            Estado nInicio = new Estado(0);

            nInicio.Transiciones.Add(new Transicion(nInicio, AFN2.Inicial, "Ɛ"));

            afn_disyun.Estados.Add(nInicio);
            afn_disyun.Inicial = nInicio;
            int i = 0;

            for (i = 0; i < AFN1.Estados.Count; i++)
            {
                Estado temp = (Estado)AFN1.Estados[i];
                temp.Id = (i + 1);
                afn_disyun.Estados.Add(temp);
            }

            for (int j = 0; j < AFN2.Estados.Count; j++)
            {
                Estado temp = (Estado)AFN2.Estados[j];
                temp.Id = (i + 1);
                afn_disyun.Estados.Add(temp);
                i++;
            }

            Estado nuevoFin = new Estado((AFN1.Estados.Count + AFN2.Estados.Count + 1));
            afn_disyun.Estados.Add(nuevoFin);
            afn_disyun.Estados_Aceptacion.Add(nuevoFin);


            Estado antInicio = AFN1.Inicial;
            List<Estado> antF1 = AFN1.Estados_Aceptacion;
            List<Estado> antF2 = AFN2.Estados_Aceptacion;

            nInicio.Transiciones.Add(new Transicion(nInicio, antInicio, "Ɛ"));

            for (int k = 0; k < antF1.Count; k++)
            {
                antF1[k].Transiciones.Add(new Transicion(antF1[k], nuevoFin, "Ɛ"));
            }

            for (int k = 0; k < antF1.Count; k++) 
            {
                antF2[k].Transiciones.Add(new Transicion(antF2[k], nuevoFin, "Ɛ"));
            }

            return afn_disyun;
        }

        public Automata cerraduraKleene(Automata automataFN)
        {
            Automata afn_ckleene = new Automata();

            Estado nuevoInicio = new Estado(0);
            afn_ckleene.Estados.Add(nuevoInicio);
            afn_ckleene.Inicial = nuevoInicio;

            for (int i = 0; i < automataFN.Estados.Count; i++)
            {
                Estado tmp = (Estado)automataFN.Estados[i];
                tmp.Id = i + 1;
                afn_ckleene.Estados.Add(tmp);
            }

            Estado nuevoFin = new Estado(automataFN.Estados.Count + 1);
            afn_ckleene.Estados.Add(nuevoFin);
            afn_ckleene.Estados_Aceptacion.Add(nuevoFin);

            Estado anteriorInicio = automataFN.Inicial;

            List<Estado> anteriorFin = automataFN.Estados_Aceptacion;

            nuevoInicio.Transiciones.Add(new Transicion(nuevoInicio, anteriorInicio, "Ɛ"));
            nuevoInicio.Transiciones.Add(new Transicion(nuevoInicio, nuevoFin, "Ɛ"));

            for (int i = 0; i < anteriorFin.Count; i++)
            {
                anteriorFin[i].Transiciones.Add(new Transicion(anteriorFin[i], anteriorInicio, "Ɛ"));
                anteriorFin[i].Transiciones.Add(new Transicion(anteriorFin[i], nuevoFin, "Ɛ"));
            }
            return afn_ckleene;
        }
    }
}
