using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _OLC1_Proyecto1_201807190
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            List<string> sPostfix = new List<string>();

            /*
            sPostfix.Add(".");
            sPostfix.Add(".");
            sPostfix.Add(".");
            sPostfix.Add("*");
            sPostfix.Add("|");
            sPostfix.Add(".");
            sPostfix.Add("c");
            sPostfix.Add("d");
            sPostfix.Add(".");
            sPostfix.Add("a");
            sPostfix.Add("b");
            sPostfix.Add("b");
            sPostfix.Add("c");
            sPostfix.Add("+");
            sPostfix.Add("d");
            */

            /*
            sPostfix.Add(".");
            sPostfix.Add("letra");
            sPostfix.Add("*");
            sPostfix.Add("|");
            sPostfix.Add("letra");
            sPostfix.Add("|");
            sPostfix.Add("digito");
            sPostfix.Add("_");
            */

            //Evaluador_Expresion myExpression = new Evaluador_Expresion(sPostfix);

            //Console.WriteLine("Resultado = " + myExpression.evaluateExpression(sPostfix));
            //Automata AFN =  myExpression.evaluateAFN(sPostfix);

            //Console.WriteLine(AFN);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
