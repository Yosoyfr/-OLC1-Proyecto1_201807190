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

        public Evaluador_Expresion(List<string> expression)
        { 
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
                    case Operador.Or:
                    case Operador.Kleen:
                    case Operador.And:
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
    }
}
