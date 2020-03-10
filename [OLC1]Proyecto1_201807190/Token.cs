using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_Proyecto1_201807190
{
    class Token
    {
        /*
       * Lista de Tokens permitidos para esta practica
       */
        public enum Tipo
        {
            Reservada_CONJ,
            Signo_Llaves_Dech,
            Signo_Llaves_Izq,
            Signo_Coma,
            Signo_Punto_y_Coma,
            Signo_Apuntador,
            Signo_Dos_Puntos,
            Signo_Punto_Concatenacion,
            Signo_Pleca_Disyuncion,
            Signo_Interrogacion,
            Signo_Asterisco,
            Signo_Mas,
            Signo_Negacion,
            Signo_Porcentaje,
            Cadena,
            Caracter,
            Todo_,
            Numero,
            Numero_Decimal,
            Variable,
            Comentario_De_Linea,
            Comentario_Multilinea,
            Desconocido,
            Reservada_No_Encontrada,
            Ultimo
        }

        /*
         * Atributos del token
         */

        public Tipo tipoToken;
        private String valor;
        private int linea;
        private int columna;

        /*
         * Constructor del Token, esperando el tipo de la lista anterior y el valor que va a tomar
         */

        public Token(Tipo tipoToken, String valor, int linea, int columna)
        {
            this.tipoToken = tipoToken;
            this.valor = valor;
            this.linea = linea;
            this.columna = columna;
        }

        /*
         * Accesores de los atributos del objeto Token
         */
        public String GetValor
        {
            get { return this.valor; }
        }

        public int GetLinea
        {
            get { return this.linea; }
        }

        public int GetColumna
        {
            get { return this.columna; }
        }

        /*
         * Dependiendo del contexto que se el tipo del token
         * devolvera el valor deseado
         */

        public String GetTipo
        {
            get
            {
                switch (tipoToken)
                {
                    /*
                     * Palabras reservadas
                     */
                    case Tipo.Reservada_CONJ:
                        return "Reservada Conjunto";
                    /*
                     * Signos
                     */
                    case Tipo.Signo_Llaves_Dech:
                        return "Llave Derecha";
                    case Tipo.Signo_Llaves_Izq:
                        return "Llave Izquierda";
                    case Tipo.Signo_Coma:
                        return "Coma";
                    case Tipo.Signo_Punto_y_Coma:
                        return "Punto y coma";
                    case Tipo.Signo_Dos_Puntos:
                        return "Dos puntos";
                    case Tipo.Signo_Apuntador:
                        return "Igual";
                    case Tipo.Signo_Punto_Concatenacion:
                        return "Punto";
                    case Tipo.Signo_Pleca_Disyuncion:
                        return "Suma";
                    case Tipo.Signo_Interrogacion:
                        return "Resta";
                    case Tipo.Signo_Asterisco:
                        return "Division";
                    case Tipo.Signo_Mas:
                        return "Multiplicacion";
                    case Tipo.Signo_Negacion:
                        return "Dos puntos";
                    case Tipo.Signo_Porcentaje:
                        return "Dos puntos";
                    /*
                     * Otros tokens
                     */
                    case Tipo.Cadena:
                        return "Cadena";
                    case Tipo.Caracter:
                        return "Caracter";
                    case Tipo.Numero:
                        return "Numero";
                    case Tipo.Numero_Decimal:
                        return "Numero Decimal";
                    case Tipo.Variable:
                        return "Variable";
                    case Tipo.Comentario_De_Linea:
                        return "Comentario de una linea";
                    case Tipo.Comentario_Multilinea:
                        return "Comentario multilinea";
                    /*
                     * Tokens desconocidos
                     */
                    case Tipo.Reservada_No_Encontrada:
                        return "Identificador desconocido";
                    default:
                        return "Desconocido";
                }
            }
        }
    }
}
