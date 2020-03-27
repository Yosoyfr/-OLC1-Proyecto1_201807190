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

        /*
         * Patron Singleton
         */
        public static Expresion instance = null;
        public static Expresion Singleton
        {
            get
            {
                if (instance == null)
                {
                    instance = new Expresion();
                }
                return instance;
            }
        }

        /*
         * Termina el Singleton
         */


        //Atributos del objeto
        string nombre;
        List<string> tokens;
        List<Token> token;
        Automata afn;
        Automata afd;

        //Lista de lexemas encontrados por todo el  analisis
        List<TokenLexema> Lexemas_Tokens;
        List<TokenLexema> Lexemas_Errores;

        //Tabla de Transiciones
        List<Tabla_Transiciones> tablaTran;

        public Expresion()
        {
            tokens = new List<string>();
            afn = new Automata();
            afd = new Automata();
            this.Nombre = "";
            tablaTran = new List<Tabla_Transiciones>();
            this.Token = new List<Token>();
            this.Lexemas_Tokens = new List<TokenLexema>();
            this.Lexemas_Errores = new List<TokenLexema>();
        }

        public Expresion(string nombre)
        {
            tokens = new List<string>();
            afn = new Automata();
            afd = new Automata();
            this.Nombre = nombre;
            tablaTran = new List<Tabla_Transiciones>();
            this.Token = new List<Token>();
            this.Lexemas_Tokens = new List<TokenLexema>();
            this.Lexemas_Errores = new List<TokenLexema>();
        }

        public List<string> Tokens { get => tokens; set => tokens = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public Automata Afn { get => afn; set => afn = value; }
        public Automata Afd { get => afd; set => afd = value; }
        public List<Token> Token { get => token; set => token = value; }

        public void convertAFN()
        {
            Queue cola = new Queue();
            List<Estado> pruebaCerr = new List<Estado>();
            pruebaCerr.Add(this.afn.Inicial);
            List<Estado> cerraduraI = cerradura(pruebaCerr);
            List<List<Estado>> List_Est = new List<List<Estado>>();
            List_Est.Add(cerraduraI);
            cola.Enqueue(cerraduraI);
            foreach (Estado est in this.afn.Estados_Aceptacion)
            {
                if (cerraduraI.Contains(est))
                {
                    this.afd.Estados_Aceptacion.Add(this.afn.Inicial);
                }
            }
            while (cola.Count > 0)
            {
                Estado newInicial = new Estado(varLetra - 65);
                Tabla_Transiciones tabla = new Tabla_Transiciones(newInicial);
                tabla.Cerraduras = List_Est[newInicial.Id];
                List<Estado> aux1 = (List<Estado>)cola.Dequeue();
                foreach (string alf in this.afd.Alfabeto)
                {
                    List<Estado> estados = move(aux1, alf);
                    int auxI = 0;
                    string auxS = "Inicio: " + (char)varLetra + " llega hasta: ";
                    string auxS1 = "";
                    int c = List_Est.Count;
                    List<Estado> newEsts = cerradura(estados);
                    bool perm = true;
                    for (int k = 0; k < c; k++)
                    {
                        if (!compareList(List_Est[k], newEsts))
                        {
                            perm = false;
                            break;
                        }
                        else
                            auxS1 = (char)(66 + auxI) + " simbolo: " + alf;
                        auxI = k;
                    }
                    if (perm)
                    {
                        List_Est.Add(newEsts);
                        cola.Enqueue(newEsts);
                    }
                    //Console.WriteLine(auxS + auxS1);
                    if (auxS1 != "")
                    {
                        Estado newFinal = new Estado(auxI + 1);
                        Transicion tran = new Transicion(newInicial, newFinal, alf);
                        newInicial.Transiciones.Add(tran);
                        tabla.Alcanzados.Add(newFinal);
                        foreach (Estado est in this.afn.Estados_Aceptacion)
                        {
                            if (newEsts.Contains(est))
                            {
                                if (!this.afd.containAceptacion(newFinal))
                                {
                                    this.afd.Estados_Aceptacion.Add(newFinal);
                                }
                            }
                        }
                    }
                    else 
                    {
                        Estado newFinal = new Estado(-1);
                        tabla.Alcanzados.Add(newFinal);
                    }
                }
                tablaTran.Add(tabla);
                this.afd.Estados.Add(newInicial);
                varLetra++;
            }
            this.afd.Inicial = this.afd.Estados[0];
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


        public List<Estado> cerradura(List<Estado> estados)
        {
            List<Estado> result = new List<Estado>();
            Stack stackEst = new Stack();
            foreach (Estado est in estados)
            {
                Estado aux = est;
                stackEst.Push(aux);

                while (stackEst.Count > 0)
                {
                    aux = (Estado)stackEst.Pop();
                    foreach (Transicion t in aux.Transiciones)
                    {
                        if (t.Simbolo.Equals("Ɛ") && !result.Contains(t.Final))
                        {
                            result.Add(t.Final);
                            stackEst.Push(t.Final);
                        }
                    }
                }
                result.Add(est);
            }
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
            foreach (Estado est in estados)
            { 
                foreach (Transicion t in est.Transiciones)
                {
                    if (t.Simbolo.Equals(simbolo))
                    {
                        alcanzados.Add(t.Final);
                    }
                }
            }
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

        public void getTabla()
        {
            string auxS = "Estado --- ";
            foreach (string varS in this.afd.Alfabeto)
            {
                auxS += varS + " --- ";
            }
            Console.WriteLine(auxS);
            Console.WriteLine(tablaTran.Count);
            foreach (Tabla_Transiciones auxTran in tablaTran)
            {
                Console.WriteLine(auxTran);
            }
        }

        public string getDOTAFN()
        {
            string graph = " digraph G {\n " +
                "rankdir=LR;\n " +
                "node [shape=doublecircle]; " + afn.getDOTAceptacion() + ";\n" +
                "node[shape = circle];\n 	" +
                "s [style=invis];\n " +
                "s-> 0[label = \"Inicio\"];\n ";

            graph += afn.getDOT();

            graph += "}";

            return graph;
        }

        public string getDOTAFD()
        {
            string graph = " digraph G {\n " +
                "rankdir=LR;\n " +
                "node [shape=doublecircle]; " + afd.getDOTAceptacion() + ";\n" +
                "node[shape = circle];\n 	" +
                "s [style=invis];\n " +
                "s-> 0[label = \"Inicio\"];\n ";

            graph += afd.getDOT();

            graph += "}";

            return graph;
        }

        public string getDOTTabla()
        {
            string graph = 
                " digraph G {\n " +
                "rankdir=LR;\n " +
                "node [shape = record, style = filled, color = white];\n " +
                "tbl [label=<\n";

            graph += "\t<table>\n";
            graph += "\t\t<tr>\n\t\t\t<td>Estados del AFN</td>\n";
            graph += "\t\t<td>Estado del AFD</td>\n";
            foreach (string varS in this.afd.Alfabeto)
            {
                graph += "\t\t\t<td>" + varS + "</td>\n";
            }
            graph += "\t\t</tr>\n";

            foreach (Tabla_Transiciones auxTran in tablaTran)
            {
                graph += "\t\t" + auxTran.getDOT() + "\n";
            }

            graph += "\t</table>\n>];\n}";

            return graph;
        }

        int estado = 0;
        int estado2 = 0;
        int cont = 0;
        int cont2 = 0;
        string lexema1 = "";
        string lexema2 = "";

        public string evaluacionLexemas(Lexema lexema)
        {
            bool bandera = true;
            bool bandera2 = false;
            string lex = lexema.Valor.Trim(new char[] { '\"' });
            if (lexema.Evaluador.Equals(this.nombre))
            {
                for (cont = 0; cont < lex.Length; cont++)
                {
                    string c = lex[cont] + "";
                    Estado est = this.afd.Estados[estado];
                    bandera = evalCadenas(est, lex, c, lexema.Conjuntos);
                    if (!bandera) 
                    {
                        break;
                    }

                }
                if (!bandera)
                {
                    bandera2 = true;
                    for (cont2 = 0; cont2 < lex.Length; cont2++)
                    {
                        string c2 = lex[cont2] + "";
                        Estado est2 = this.afd.Estados[estado2];
                        bandera2 = evalCadenas1(est2, lex, c2, lexema.Conjuntos);
                        if (!bandera2)
                        {
                            break;
                        }

                    }
                }
                bool perm = false;
                foreach (Estado est in this.afd.Estados_Aceptacion)
                {
                    if (estado == est.Id || estado2 == est.Id)
                    {
                        perm = true;
                        break;
                    }
                }

                if (!bandera && !bandera2)
                    addTokenErroneo(_OLC1_Proyecto1_201807190.Token.Tipo.Desconocido, lexema1, cont, lex);

                estado = 0;
                cont = 0;
                estado2 = 0;
                cont2 = 0;
                if (perm && (bandera || bandera2))
                {
                    return "      " + lexema.Valor + " lexema valido para la expresion: " + this.nombre + "\n";
                }
                else
                {
                    return "      " + lexema.Valor + " lexema no valido para la expresion: " + this.nombre + "\n";
                }
            }
            return "";
        }

        public TokenLexema addToken(Token.Tipo token, String lexema, int columna, string lexemaBase)
        {
            TokenLexema nuevoToken = new TokenLexema(new Token(token, lexema, 0, columna), lexemaBase);
            Lexemas_Tokens.Add(nuevoToken);
            //Console.WriteLine(nuevoToken.Token.GetValor + " Token de: " + nuevoToken.Expresion);
            return nuevoToken;
        }

        public TokenLexema addTokenErroneo(Token.Tipo token, String lexema, int columna, string lexemaBase)
        {
            TokenLexema nuevoToken = new TokenLexema(new Token(token, lexema, 0, columna), lexemaBase);
            Lexemas_Errores.Add(nuevoToken);
            //Console.WriteLine("Error con " + nuevoToken.Token.GetValor + " para el lexema: " + nuevoToken.Expresion);
            return nuevoToken;
        }

        public List<TokenLexema> exportListLexeme() 
        {
            return Lexemas_Tokens;
        }

        public List<TokenLexema> exportListError()
        {
            return Lexemas_Errores;
        }

        private bool evalCadenas(Estado est, string lex, string c, List<Conjunto> listConj)
        {
            foreach (Transicion t in est.Transiciones)
            {
                bool esConjunto = false;
                Conjunto conjuntoAux = null;
                foreach (Conjunto conj in listConj)
                {
                    if (t.Simbolo.Equals(conj.Nombre))
                    {
                        esConjunto = true;
                        conjuntoAux = conj;
                        break;
                    }
                }
                if (esConjunto)
                {
                    foreach (char cAux in conjuntoAux.ArrayValue)
                    {
                        lexema1 = c[0].ToString();
                        if (cAux == c[0])
                        {
                            estado = t.Final.Id;
                            addToken(_OLC1_Proyecto1_201807190.Token.Tipo.Desconocido, lexema1, cont, lex);
                            return true;
                        }
                    }
                }
                else 
                {
                    char[] todo = { '[', ']', ':'};
                    int j = cont;   
                    if (t.Simbolo.Trim(new char[] { '\"' }).TrimStart(todo).TrimEnd(todo).Length != 1)
                    {
                        string prueba = "";
                        for (j = cont; j < cont + t.Simbolo.Trim(new char[] { '\"' }).TrimStart(todo).TrimEnd(todo).Length; j++)
                        {
                            if (j < lex.Length)
                                prueba += lex[j];
                        }
                        c = prueba;
                        j--;
                    }
                    lexema1 = c;
                    if (estado == t.Inicial.Id && c == t.Simbolo.Trim(new char[] { '\"' }).TrimStart(todo).TrimEnd(todo))
                    {
                        cont = j;
                        estado = t.Final.Id;
                        addToken(_OLC1_Proyecto1_201807190.Token.Tipo.Desconocido, lexema1, cont, lex);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool evalCadenas1(Estado est, string lex, string c, List<Conjunto> listConj)
        {
            est.Transiciones.Reverse();
            foreach (Transicion t in est.Transiciones)
            {
                bool esConjunto = false;
                Conjunto conjuntoAux = null;
                foreach (Conjunto conj in listConj)
                {
                    if (t.Simbolo.Equals(conj.Nombre))
                    {
                        esConjunto = true;
                        conjuntoAux = conj;
                        break;
                    }
                }
                if (esConjunto)
                {
                    foreach (char cAux in conjuntoAux.ArrayValue)
                    {
                        lexema2 = c.ToString();
                        if (cAux == c[0])
                        {
                            estado2 = t.Final.Id;
                            addToken(_OLC1_Proyecto1_201807190.Token.Tipo.Desconocido, lexema2, cont2, lex);
                            return true;
                        }
                    }
                }
                else
                {
                    int j = cont2;
                    if (t.Simbolo.Trim(new char[] { '\"' }).Length != 1)
                    {
                        string prueba = "";
                        for (j = cont2; j < cont2+ t.Simbolo.Trim(new char[] { '\"' }).Length; j++)
                        {
                            if (j < lex.Length)
                                prueba += lex[j];
                        }
                        c = prueba;
                        j--;
                    }
                    lexema2 = c;
                    if (estado2 == t.Inicial.Id && c == t.Simbolo.Trim(new char[] { '\"' }))
                    {
                        cont2 = j;
                        estado2 = t.Final.Id;
                        addToken(_OLC1_Proyecto1_201807190.Token.Tipo.Desconocido, lexema2, cont2, lex);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
