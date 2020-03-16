    using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_Proyecto1_201807190
{
    class Node
    {
        public string data;
        public string id;
        public Node right = null;
        public Node left = null;

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

        //Proceso para generar el arbol
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
                case Operador.Or:
                    result += ")" + (string)stackResults.Pop() + "(+";
                    break;
                case Operador.Kleen:
                    result += ")" + (string)stackResults.Pop() + "(*";
                    break;
                case Operador.And:
                    result += ")" + (string)stackResults.Pop() + "(?";
                    break;
                default:
                    result += data;
                    break;
            }
            stackResults.Push(result);
        }
    }
}
