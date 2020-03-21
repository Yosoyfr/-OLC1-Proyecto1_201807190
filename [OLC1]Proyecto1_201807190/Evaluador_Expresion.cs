using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_Proyecto1_201807190
{
    //Clase para evaluar la expresion que se ingrese
    class Evaluador_Expresion: Operador
    {
        //Nodo para la creacion del arbol a partir de la expresion polaca
        Node preASTTree = null;

        //Pila de nodos que contendra el arbol
        Stack stackNodes = null;

        //Pila de resultados para obtener la expresion Inorder
        Stack stackResults = null;
        Stack stackAFN = null;

        public Evaluador_Expresion(List<string> expression)
        {
            int i = 0;
            int cont = 0;
            while (i < expression.Count)
            {
                if (expression[i].Equals("+"))
                    cont++;
                i++;
            }
            Stack p1 = new Stack();
            Stack p2 = new Stack();

            while (cont > 0)
            {
                convertList(expression, p1, p2);
                cont--;
            }
            preASTTree = createPreExp(expression);
        }


        //Creacion del arbol
        private Node createPreExp(List<string> expression)
        {
            //Se lee la expresion de izquierda a derecha
            int i = expression.Count-1;
            stackNodes = new Stack();

            while (i > -1)
            {
                string token = expression[i--];

                switch (token)
                {
                    //Si son los operadores *, +, ? solo tiene un nodo hijo
                    case Operador.Cerradura_Positiva:
                    case Operador.Kleene:
                    case Operador.Interrogacion:
                        //Se saca un operando de la pila como el hijo
                        Node sonOperand = (Node)stackNodes.Pop();

                        //Se realiza la operacion de un solo hijo
                        Node unitOperation = new Node(token, sonOperand);

                        //Y se agrega a la pila de nodos
                        stackNodes.Push(unitOperation);
                        break;

                    //Si son los operadores ., | tienen dos nodos hijos (derecha e izquierda)
                    case Operador.Concatenar:
                    case Operador.Disyuncion:

                        //Se sacan dos operandos de la pila sinedo los dos hijos (derecha e izquierda)
                        Node leftOperand = (Node)stackNodes.Pop();
                        Node rightOperand = (Node)stackNodes.Pop();

                        //Se realiza la operacion de dos hijos (binario)
                        Node binaryOperation = new Node(token, rightOperand, leftOperand);

                        // Y se agrega a la pila de nodos
                        stackNodes.Push(binaryOperation);
                        break;

                    //Si no son operadores entoncces se agregan a la pila para esperar ser sacados
                    //como hijos
                    default:

                        //El operando es un termino que no tendra hijos
                        Node operand = new Node(token);

                        // se agrega a la pila de nodos
                        stackNodes.Push(operand);
                        break;
                }
            }

            //Retornamos el ultimo nodo apilado
            return (Node)stackNodes.Pop();
        }

        //Metodo para convertir la cerradura positiva (+) en (.) expresion (*) expresion
        public void convertList(List<string> expression, Stack p1, Stack p2) 
        {
            //Se lee la expresion de izquierda a derecha
            int i = expression.Count - 1;
            while (i > -1)
            {
                string token = expression[i];

                if (token.Equals("+"))
                {
                    expression.RemoveAt(i);

                    expression.Add(".");
                    while(p1.Count > 0)
                        expression.Add((string)p1.Pop());

                    expression.Add("*");
                    while (p2.Count > 0)
                        expression.Add((string)p2.Pop());
                    break;
                }
                else 
                {
                    p1.Push(token);
                    p2.Push(token);
                    expression.RemoveAt(i);
                }
                i--;
            }
        }

        public string evaluateExpression(List<string> expression)
        {
            string result = "";
            stackResults = new Stack();

            preASTTree = createPreExp(expression);
            try
            {
                preASTTree.processNode(stackResults);
                result = (string)stackResults.Pop();
            }
            catch (Exception) 
            { 

            }
            string resultado = "";
            int i = result.Length - 1;
            while (i > -1)
            {
                resultado += result[i--];
            }
            return resultado;
        }

        public Automata evaluateAFN(List<string> expression)
        {
            Automata result = new Automata();
            stackAFN = new Stack();

            preASTTree = createPreExp(expression);
            try
            {
                preASTTree.buildAFN(stackAFN);
                result = (Automata)stackAFN.Pop();
            }
            catch (Exception)
            {

            }

            return result;
        }
    }
}
