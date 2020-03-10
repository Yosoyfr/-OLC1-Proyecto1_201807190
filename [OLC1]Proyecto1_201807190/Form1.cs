using System;
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

        public void correrAnalisis()
        {
            String entrada = textualTabControl1.SelectedTab.Controls[0].Text;

            /*
             * Proceso de análisis léxico
             */
            List<Token> tokensAnalisis = Analizador_Lexico.Singleton.analizador(entrada);

            tokensAnalisis.Add(new Token(Token.Tipo.Ultimo, "ultimo", 0, 0));

            Analizador_Lexico.Singleton.imprimirListaToken(this.nombreArchivoER);
            Analizador_Lexico.Singleton.imprimirListaErrores(this.nombreArchivoER);
            Console.WriteLine("FIN!!!");
        }
    }
}
