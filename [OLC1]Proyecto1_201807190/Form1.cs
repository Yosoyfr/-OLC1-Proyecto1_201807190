using _OLC1_Proyecto1_201807190.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _OLC1_Proyecto1_201807190
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /*
         * Metodo para crear nuevas Pestañas
         */

        private void agregarPestañaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String title = "Pestaña " + (textualTabControl1.TabCount + 1).ToString();
            TabPage myTabPage = new TabPage(title);
            textualTabControl1.TabPages.Add(myTabPage);
            textualTabControl1.SelectedTab = myTabPage;
        }

        /*
         * Metodo para el tipo de rutas
         */

        static RutasArchivos[] arrayRutas = new RutasArchivos[100];

        public void nuevaRuta(String nombreArchivo, String rutaArchibo)
        {
            for (int i = 0; i < arrayRutas.Length; i++)
            {
                if (arrayRutas[i] == null)
                {
                    RutasArchivos nuevoArchivo = new RutasArchivos(i + 1 + " - " + nombreArchivo, rutaArchibo);
                    arrayRutas[i] = nuevoArchivo;
                    textualTabControl1.SelectedTab.Text = nuevoArchivo.NombreArchivo;
                    break;
                }
            }
        }

        private void abrirArchivoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog Archivo = new OpenFileDialog();
            Archivo.Filter = "Archivo ER |*.er";
            Archivo.InitialDirectory = @"C:\Users\Francisco Suarez\Desktop";

            if (Archivo.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    textualTabControl1.SelectedTab.Controls[0].Text = File.ReadAllText(Archivo.FileName);
                    nuevaRuta(Archivo.SafeFileName, Archivo.FileName);
                }
                catch (ArgumentNullException)
                {
                    Console.WriteLine("Hubo un error");
                }
            }
        }

        Boolean stepSave = true;
        String rutaPrevia = "";

        private void guardarArchivoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*
          * Creacion del contenido archivo.er
          */
            String Contenido_Archivo_ER = "";
            try
            {
                /*
                * Obtenemos el contenido del textBox de la pestaña
                */
                Contenido_Archivo_ER = textualTabControl1.SelectedTab.Controls[0].Text;
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Hubo un error");
            }

            for (int i = 0; i < arrayRutas.Length; i++)
            {
                if (arrayRutas[i] != null && arrayRutas[i].NombreArchivo == textualTabControl1.SelectedTab.Text)
                {
                    stepSave = false;
                    rutaPrevia = arrayRutas[i].RutaArchivo;
                }
            }

            SaveFileDialog archivoER = new SaveFileDialog();
            archivoER.Filter = "Archivo ER |*.er";
            archivoER.InitialDirectory = @"C:\Users\Francisco Suarez\Desktop";
            if (stepSave)
            {
                archivoER.FileName = "Archivo";

                if (archivoER.ShowDialog() == DialogResult.OK && archivoER.FileName != null)
                {
                    File.WriteAllText(archivoER.FileName, Contenido_Archivo_ER);
                    nuevaRuta(Path.GetFileName(archivoER.FileName), archivoER.FileName);
                }
            }
            else
            {
                File.WriteAllText(rutaPrevia, Contenido_Archivo_ER);
            }
            stepSave = true;
            rutaPrevia = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.correrAnalisis();
        }

        public string nombreArchivoER = "";

        List<Image> imagesAFN = new List<Image>();
        List<Image> imagesAFD = new List<Image>();
        List<Image> imagesTran = new List<Image>();

        List<TokenLexema> Lexemas_Tokens = new List<TokenLexema>();
        List<TokenLexema> Lexemas_Errores = new List<TokenLexema>();
        public void correrAnalisis()
        {
            imagesAFN = new List<Image>();
            imagesAFD = new List<Image>();
            imagesTran = new List<Image>();
            Lexemas_Tokens = new List<TokenLexema>();
            Lexemas_Errores = new List<TokenLexema>();
            String entrada = textualTabControl1.SelectedTab.Controls[0].Text;
            /*
             * Proceso de análisis léxico
             */
            Analizador_Lexico analisis = new Analizador_Lexico();
            List<Token> tokensAnalisis = analisis.analizador(entrada);
            analisis.imprimirXMLErrores("Errores");
            analisis.imprimirListaToken(this.nombreArchivoER, tokensAnalisis);
            analisis.imprimirListaErrores(this.nombreArchivoER);
            analisis.imprimirXML(tokensAnalisis, "Tokens");
            analisis.distribucionConjuntos();
            Console.WriteLine("Conjuntos: ");
            analisis.getConjuntos();
            Console.WriteLine("Lexemas: ");
            List<Lexema> lexemasAnalisis =  analisis.distribucionLexemas();
            tokensAnalisis.Add(new Token(Token.Tipo.Ultimo, "ultimo", 0, 0));

            //Lista de expresiones
            List<Expresion> expresiones = new List<Expresion>();

            int i = 0;

            while (i < tokensAnalisis.Count) 
            {
                if (tokensAnalisis[i].tipoToken == Token.Tipo.Variable && tokensAnalisis[i + 1].tipoToken == Token.Tipo.Signo_Flecha && tokensAnalisis[i + 2].tipoToken != Token.Tipo.Valor_CONJ) 
                {
                    Expresion exp = new Expresion(tokensAnalisis[i].GetValor);
                    int j = i + 2;
                    try
                    {
                        while (tokensAnalisis[j].tipoToken != Token.Tipo.Signo_Punto_y_Coma)
                        {
                            if (tokensAnalisis[j].tipoToken == Token.Tipo.Signo_Llaves_Dech || tokensAnalisis[j].tipoToken == Token.Tipo.Signo_Llaves_Izq)
                            { }
                            else
                            {
                                exp.Tokens.Add(tokensAnalisis[j].GetValor);
                            }
                            j++;
                        }
                        
                        Evaluador_Expresion myExpression = new Evaluador_Expresion(exp.Tokens);
                        List<Token> auxTokens = analisis.analizador(myExpression.evaluateExpression(exp.Tokens));
                        exp.Tokens.Clear();
                        foreach (Token t in auxTokens)
                        {
                            exp.Tokens.Add(t.GetValor);
                        }
                        exp.Token = auxTokens;
                        expresiones.Add(exp);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Hace falta un punto y coma", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                    }
                }
                i++;
            }
            string consola = "   Analisis de lexemas por parte de las expresiones regulares\n";
            
            for (int j = 0; j < expresiones.Count; j++)
            {
                Evaluador_Expresion myExpression = new Evaluador_Expresion(expresiones[j].Tokens);

                Automata AFN = new Automata();
                AFN = myExpression.evaluateAFN(expresiones[j].Tokens);

                List<string> alfabeto = new List<string>();

                foreach (string token in expresiones[j].Tokens)
                {
                    if (!alfabeto.Contains(token) && !token.Equals("Ɛ") && !token.Equals("+") && !token.Equals("|") && !token.Equals("*") && !token.Equals("?") && !token.Equals("."))
                    {
                        alfabeto.Add(token);
                    }
                }

                expresiones[j].Afn = AFN;
                expresiones[j].Afd.Alfabeto = alfabeto;
                expresiones[j].convertAFN();
                /*
                Console.WriteLine(expresiones[j].Afn);
                Console.WriteLine(expresiones[j].Afd);
                */
                imagesAFN.Add(imge(expresiones[j].getDOTAFN()));
                imagesAFD.Add(imge(expresiones[j].getDOTAFD()));
                imagesTran.Add(imge(expresiones[j].getDOTTabla()));

                int a = 0;
                int c = 8;
                bool b = (a == c && a <= 8) || a != 0;
                foreach (Lexema lex in lexemasAnalisis)
                {
                    consola += expresiones[j].evaluacionLexemas(lex);
                }
                foreach (TokenLexema t in expresiones[j].exportListLexeme())
                {
                    Lexemas_Tokens.Add(t);
                }
                foreach (TokenLexema t in expresiones[j].exportListError())
                {
                    Lexemas_Errores.Add(t);
                }
            }
            try 
            {
                richTextBox1.Text = consola;
                pictureBox1.Image = imagesAFD[0];
                pictureBox2.Image = imagesAFN[0];
                pictureBox3.Image = imagesTran[0];
                analisis.imprimirXMLLexemas("Tokens_Lexemas", Lexemas_Tokens);
                analisis.imprimirXMLLexemasErrores("Errores_Lexemas", Lexemas_Errores);
            }
            catch (Exception)
            {
                pictureBox1.Image = Resources.error;
                pictureBox2.Image = Resources.error;
                pictureBox3.Image = Resources.error_1;
            }
            Console.WriteLine("FIN!!!");
        }

        public static string graphviz = @"D:\Graphviz2.38\bin\dot.exe";
        public static string archivoentrada = @"D:\Francisco" + '\\' + "afn.dot";
        private Bitmap imge(string dot)
        {
            try
            {
                string executable = graphviz;
                string output = archivoentrada;
                File.WriteAllText(output, dot);

                System.Diagnostics.Process process = new System.Diagnostics.Process();

                // Stop the process from opening a new window
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                // Setup executable and parameters
                process.StartInfo.FileName = executable;
                process.StartInfo.Arguments = string.Format(@"{0} -Tjpg -O", output);

                // Go
                process.Start();
                // and wait dot.exe to complete and exit
                process.WaitForExit();
                Bitmap bitmap = null; ;
                using (Stream bmpStream = System.IO.File.Open(output + ".jpg", System.IO.FileMode.Open))
                {
                    Image image = Image.FromStream(bmpStream);
                    bitmap = new Bitmap(image);
                }

                string path = output + ".jpg";
                File.Delete(output + ".jpg");
                return bitmap;
            }
            catch (System.IO.FileNotFoundException)
            {
                MessageBox.Show("Algo ha salido mal", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return Resources.error;
        }

        int contImages = 0;
        private void button2_Click(object sender, EventArgs e)
        {
            try 
            {
            if (contImages == 0)
                return;
            else
                contImages--;
            pictureBox1.Image = imagesAFD[contImages];
            pictureBox2.Image = imagesAFN[contImages];
            pictureBox3.Image = imagesTran[contImages];
            }
            catch (Exception)
            { }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try 
            {
            if(imagesAFD.Count > 0)
                if (contImages == imagesAFD.Count - 1)
                    return;
                else
                    contImages++;
            pictureBox1.Image = imagesAFD[contImages];
            pictureBox2.Image = imagesAFN[contImages];
            pictureBox3.Image = imagesTran[contImages];
            }
            catch (Exception)
            { }
        }
    }
}
