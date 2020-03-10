using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_Proyecto1_201807190
{
    class RutasArchivos
    {
        /*
        * Atributos del objeto
        */

        private String nombreArchivo, rutaArchivo;

        /*
         * Constructor del objeto 
         */

        public RutasArchivos(String numeroDia, String descripcion)
        {
            this.nombreArchivo = numeroDia;
            this.rutaArchivo = descripcion;
        }

        /*
         * Accesores y modificadores de todos los atributos
         */


        public String NombreArchivo
        {
            set { this.nombreArchivo = value; }
            get { return this.nombreArchivo; }
        }

        public String RutaArchivo
        {
            set { this.rutaArchivo = value; }
            get { return this.rutaArchivo; }
        }
    }
}
