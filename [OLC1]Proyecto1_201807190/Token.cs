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
            Signo_Flecha,
            Signo_Dos_Puntos,
            Operador_Punto,
            Operador_Disyuncion,
            Operador_Interrogacion,
            Operador_Asterisco,
            Operador_Mas,
            Cadena,
            /*
            Caracter,
            Numero,
            Numero_Decimal,
            */
            Variable,
            Comentario_De_Linea,
            Comentario_Multilinea,
            Valor_CONJ,
            Salto_de_Linea,
            Comilla_Simple,
            Comilla_Doble,
            Tabulacion,
            Todo_,
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
                    case Tipo.Signo_Flecha:
                        return "Flecha";
                    case Tipo.Operador_Punto:
                        return "Operador Concatenar";
                    case Tipo.Operador_Disyuncion:
                        return "Operador Disyuncion (OR)";
                    case Tipo.Operador_Interrogacion:
                        return "Operador 0 o 1 vez";
                    case Tipo.Operador_Asterisco:
                        return "Operador Cerradura 0 o mas veces";
                    case Tipo.Operador_Mas:
                        return "Operador 1 o mas veces";
                    /*
                     * Otros tokens
                     */
                    case Tipo.Cadena:
                        return "Cadena";
                    case Tipo.Variable:
                        return "Identificador";
                    case Tipo.Comentario_De_Linea:
                        return "Comentario de una linea";
                    case Tipo.Comentario_Multilinea:
                        return "Comentario multilinea";
                    case Tipo.Valor_CONJ:
                        return "Valor del conjunto";
                    case Tipo.Salto_de_Linea:
                        return "Salto de Linea";
                    case Tipo.Tabulacion:
                        return "Tabulacion";
                    case Tipo.Comilla_Simple:
                        return "Comilla simple";
                    case Tipo.Comilla_Doble:
                        return "Comillas dobles";
                    case Tipo.Todo_:
                        return "Todo";
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
