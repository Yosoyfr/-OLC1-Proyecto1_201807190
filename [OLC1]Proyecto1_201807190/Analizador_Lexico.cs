using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _OLC1_Proyecto1_201807190
{
    class Analizador_Lexico
    {
        /*
         * Patron Singleton
         */
        public static Analizador_Lexico instance = null;
        public static Analizador_Lexico Singleton
        {
            get
            {
                if (instance == null)
                {
                    instance = new Analizador_Lexico();
                }
                return instance;
            }
        }

        /*
         * Termina el Singleton
         */

        /*
         * Variable que representa la lista de tokens
         */
        public List<Token> Lista_de_Tokens;

        /*
        * Variable que representa la lista de tokens
        */
        private List<Token> Lista_de_Errores;


        /*
         * Variable booleana que me da lugar a crear el dot
         */

        private Boolean pasoLibre;

        public Analizador_Lexico()
        {
            pasoLibre = true;
        }

        public Boolean stepHTML()
        {
            if (pasoLibre)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /*
         * Metodo para añadir token a la lista de tokens
         */

        public void addToken(Token.Tipo token, String lexema, int linea, int columna)
        {
            Token nuevoToken = new Token(token, lexema, linea, columna);
            Lista_de_Tokens.Add(nuevoToken);
            Console.WriteLine(nuevoToken.GetTipo);
        }

        /*
         * Metodo para añadir error a la lista de tokens errorenos
         */

        public void addTokenErroneo(Token.Tipo token, String lexema, int linea, int columna)
        {
            Token nuevoToken = new Token(token, lexema, linea, columna);
            Lista_de_Errores.Add(nuevoToken);
        }

        public List<Token> analizador(String entrada)
        {
            Lista_de_Tokens = new List<Token>();
            Lista_de_Errores = new List<Token>();
            int estado = 0;
            int columna = 0;
            int fila = 1;
            String lexema = "";
            Char c;
            bool Conj = false;
            bool id = false;
            bool flecha_ = false;
            bool dos_puntos = false;
            entrada = entrada + " ";
            pasoLibre = true;
            for (int i = 0; i < entrada.Length; i++)
            {
                c = entrada[i];
                columna++;
                switch (estado)
                {
                    case 0:
                        /*
                         * Revisara si puede ser un valor cunjunto
                         */                       
                        if (((int)c >= 33 && (int)c <= 125) && Conj && dos_puntos && id && flecha_)
                        {
                            estado = 2;
                            lexema += c;
                        }
                        /*
                        * Revisara si puede ser una palabra reservada, un caracter o una variable
                        */
                        else if (Char.IsLetter(c))
                        {
                            estado = 1;
                            lexema += c;
                        }
                        /*
                         * Revisara si puede ser una cadena
                         */
                        else if (c == '"')
                        {
                            estado = 5;
                            i--;
                            columna--;
                        }
                        else if (c == '[')
                        {
                            estado = 19;
                            lexema += c;
                        }
                        /*
                         * Revisara si puede ser un espacio en blanco
                         */
                        else if (c == ' ')
                        {
                            estado = 0;
                        }
                        /*
                         * Revisara si puede ser un enter, para cambiar de linea
                         */
                        else if (c == '\n')
                        {
                            columna = 0;
                            fila++;
                            estado = 0;
                        }

                        /*
                         * Lista de Tokens ya establecidos que son todos los simbolos admitidos
                         */
                        else if (c.CompareTo('{') == 0)
                        {
                            lexema += c;
                            addToken(Token.Tipo.Signo_Llaves_Izq, lexema, fila, columna);
                            lexema = "";
                        }
                        else if (c.CompareTo('}') == 0)
                        {
                            lexema += c;
                            addToken(Token.Tipo.Signo_Llaves_Dech, lexema, fila, columna);
                            lexema = "";
                        }
                        else if (c.CompareTo(',') == 0)
                        {
                            lexema += c;
                            addToken(Token.Tipo.Signo_Coma, lexema, fila, columna);
                            lexema = "";
                        }
                        else if (c.CompareTo(';') == 0)
                        {
                            lexema += c;
                            addToken(Token.Tipo.Signo_Punto_y_Coma, lexema, fila, columna);
                            lexema = "";
                        }
                        else if (c.CompareTo('-') == 0)
                        {
                            estado = 8;
                            lexema += c;
                        }
                        else if (c.CompareTo(':') == 0)
                        {
                            lexema += c;
                            addToken(Token.Tipo.Signo_Dos_Puntos, lexema, fila, columna);
                            dos_puntos = true;
                            lexema = "";
                        }
                        else if (c.CompareTo('.') == 0)
                        {
                            lexema += c;
                            addToken(Token.Tipo.Operador_Punto, lexema, fila, columna);
                            lexema = "";
                        }
                        else if (c.CompareTo('|') == 0)
                        {
                            lexema += c;
                            addToken(Token.Tipo.Operador_Disyuncion, lexema, fila, columna);
                            lexema = "";
                        }
                        else if (c.CompareTo('?') == 0)
                        {
                            lexema += c;
                            addToken(Token.Tipo.Operador_Interrogacion, lexema, fila, columna);
                            lexema = "";
                        }
                        else if (c.CompareTo('*') == 0)
                        {
                            lexema += c;
                            addToken(Token.Tipo.Operador_Asterisco, lexema, fila, columna);
                            lexema = "";
                        }
                        else if (c.CompareTo('+') == 0)
                        {
                            lexema += c;
                            addToken(Token.Tipo.Operador_Mas, lexema, fila, columna);
                            lexema = "";
                        }
                        else if (c.CompareTo('/') == 0)
                        {
                            estado = 9;
                            lexema += c;
                        }
                        else if (c.CompareTo('\\') == 0)
                        {
                            estado = 3;
                            lexema += c;
                        }
                        else if (c.CompareTo('<') == 0)
                        {
                            estado = 10;
                            lexema += c;
                        }

                        /*
                         * Si no es ninguno de la lista de tokens, nos devuelve un error
                         */

                        else
                        {
                            estado = -1;
                            i--;
                            columna--;
                        }
                        break;

                    case 1:
                        /*
                         * Buscara que palabra reservada es
                         */
                        if (Char.IsLetterOrDigit(c) || c == '_')
                        {
                            lexema += c;
                            estado = 1;
                        }
                        else if (lexema.Equals("CONJ"))
                        {
                            addToken(Token.Tipo.Reservada_CONJ, lexema, fila, columna);
                            Conj = true;
                            lexema = "";
                            i--;
                            columna--;
                            estado = 0;
                        }
                        /*
                         * Si no encuentra resultados, esta palabra es una varible
                         */

                        else
                        {
                            addToken(Token.Tipo.Variable, lexema, fila, columna);
                            id = true;
                            lexema = "";
                            i--;
                            columna--;
                            estado = 0;
                        }
                        break;
                    case 2:
                        /*
                         * Revisara el valor conjunto
                         */
                        
                        if (((int)c >= 33 && (int)c <= 126) && c != ';')
                        {
                            lexema += c;
                            estado = 2;
                        }
                        else
                        {
                            addToken(Token.Tipo.Valor_CONJ, lexema, fila, columna);
                            lexema = "";
                            i--;
                            columna--;
                            estado = 0;
                            Conj = false;
                            id = false;
                            flecha_ = false;
                            dos_puntos = false;
                        }
                        
                        break;
                    case 3:
                        /*
                        * Comprobar que es un comentario multilinea
                        */
                        if (c == 'n' || c == '\'' || c == '\"' || c == 't')
                        {
                            lexema += c;
                            estado = 3;
                        }
                        else if (lexema.Equals("\\n"))
                        {
                            addToken(Token.Tipo.Salto_de_Linea, lexema, fila, columna);
                            lexema = "";
                            i--;
                            columna--;
                            estado = 0;
                        }
                        else if (lexema.Equals("\\t"))
                        {
                            addToken(Token.Tipo.Tabulacion, lexema, fila, columna);
                            lexema = "";
                            i--;
                            columna--;
                            estado = 0;
                        }
                        else if (lexema.Equals("\\\'"))
                        {
                            addToken(Token.Tipo.Comilla_Simple, lexema, fila, columna);
                            lexema = "";
                            i--;
                            columna--;
                            estado = 0;
                        }
                        else if (lexema.Equals("\\\""))
                        {
                            addToken(Token.Tipo.Comilla_Doble, lexema, fila, columna);
                            lexema = "";
                            i--;
                            columna--;
                            estado = 0;
                        }
                        else
                        {
                            estado = -1;
                            i--;
                            columna--;
                        }
                        break;
                    case 4:

                        break;
                    case 5:
                        /*
                         * Comprueba que es una cadena
                         */
                        if (c == '"')
                        {
                            lexema += c;
                            estado = 6;
                        }
                        break;
                    case 6:
                        /*
                         * Comprobara todos los datos que contendra la cadena,
                         * hasta encontrar otro (") para cerrar la cadena
                         */
                        if (c == '\n')
                        {
                            columna = 0;
                            fila++;
                            estado = 6;
                        }
                        else if (c != '"')
                        {
                            lexema += c;
                            estado = 6;
                        }
                        else
                        {
                            estado = 7;
                            i--;
                            columna--;
                        }
                        break;
                    case 7:
                        /*
                         * Aqui cierra la cadena al encontrar (")
                         */
                        if (c == '"')
                        {
                            lexema += c;
                            addToken(Token.Tipo.Cadena, lexema, fila, columna);
                            estado = 0;
                            lexema = "";
                        }
                        break;

                        /*
                        * Sentencias de dobles simbolos
                        */
                    case 8:
                        if (c.CompareTo('>') == 0 && lexema.Length < 2)
                        {
                            lexema += c;
                            estado = 8;
                        }
                        else if (lexema.Equals("->"))
                        {
                            addToken(Token.Tipo.Signo_Flecha, lexema, fila, columna);
                            flecha_ = true;
                            lexema = "";
                            i--;
                            columna--;
                            estado = 0;
                        }
                        else
                        {
                            estado = -1;
                            i--;
                            columna--;
                        }
                        break;
                        /*
                        * Comentarios
                        */
                    case 9:
                        /*
                        * Comprueba que es un comentario de una linea
                        */
                        lexema += c;
                        if (lexema.Equals("//"))
                        {
                            estado = 13;
                        }
                        else
                        {
                            estado = -1;
                            i--;
                            columna--;
                        }
                        break;
                    case 10:
                        /*
                        * Comprobar que es un comentario multilinea
                        */
                        if (c == '!')
                        {
                            lexema += c;
                            if (lexema.Equals("<!"))
                            {
                                estado = 11;
                            }
                            else
                            {
                                estado = 10;
                            }
                        }
                        else 
                        {
                            estado = -1;
                            i--;
                            columna--;
                        }
                        break;
                    case 11:
                        /*
                        * Comprobar que es un comentario multilinea
                        */
                        if (c == '\n')
                        {
                            columna = 0;
                            fila++;
                            estado = 11;
                        }
                        else if (c != '!')
                        {
                            lexema += c;
                            estado = 11;
                        }
                        else
                        {
                            estado = 12;
                            i--;
                            columna--;
                        }
                        break;
                    case 12:
                        /*
                         * Aqui cierra el comentario multilinea
                        */
                        if (c == '!')
                        {
                            lexema += c;
                            estado = 12;
                        }
                        else if (c != '>')
                        {
                            lexema += c;
                            estado = 11;
                        }
                        else
                        {
                            lexema += c;
                            addToken(Token.Tipo.Comentario_Multilinea, lexema.Replace('<', ' ').Replace('>', ' ').Replace('!', ' '), fila, columna);
                            lexema = "";
                            columna--;
                            estado = 0;
                        }
                        break;
                    case 13:
                        /*
                         * Aqui cierra el comentario linea
                        */
                        if (c == '\n')
                        {
                            lexema += c;
                            addToken(Token.Tipo.Comentario_De_Linea, lexema, fila, columna);
                            lexema = "";
                            i--;
                            estado = 0;

                        }
                        else if (i == entrada.Length - 1)
                        {
                            lexema += c;
                            addToken(Token.Tipo.Comentario_De_Linea, lexema, fila, columna);
                            lexema = "";
                            i--;
                            estado = 0;
                        }
                        else
                        {
                            lexema += c;
                            estado = 13;
                        }
                        break;
                    case 19:
                        /*
                        * Comprobar que es un [:TODO:]
                        */
                        if (c == ':')
                        {
                            lexema += c;
                            if (lexema.Equals("[:"))
                            {
                                estado = 20;
                            }
                            else
                            {
                                estado = 19;
                            }
                        }
                        else
                        {
                            estado = -1;
                            i--;
                            columna--;
                        }
                        break;
                    case 20:
                        /*
                        * Comprobar que es puede recibir cualquier cosa
                        */
                        if (c == '\n')
                        {
                            columna = 0;
                            fila++;
                            estado = 20;
                        }
                        else if (c != ':')
                        {
                            lexema += c;
                            estado = 20;
                        }
                        else
                        {
                            estado = 21;
                            i--;
                            columna--;
                        }
                        break;
                    case 21:
                        /*
                         * Aqui cierra el comentario [:TODO:]
                        */
                        if (c == ':')
                        {
                            lexema += c;
                            estado = 21;
                        }
                        else if (c != ']')
                        {
                            lexema += c;
                            estado = 20;
                        }
                        else
                        {
                            lexema += c;
                            addToken(Token.Tipo.Todo_, lexema, fila, columna);
                            lexema = "";
                            columna--;
                            estado = 0;
                        }
                        break;
                    /*
                     * Si no se cumple con ninguno de estos tipos 
                     * de tokens, devuelve un error exceptuado 
                     * saltos de linea, espacios o tabulaciones
                     */
                    case -1:
                        lexema += c;
                        Console.WriteLine("Error lexico con: " + lexema);
                        addTokenErroneo(Token.Tipo.Desconocido, lexema, fila, columna);
                        estado = 0;
                        lexema = "";
                        pasoLibre = false;
                        break;
                }

            }
            if (pasoLibre)
            {
                return Lista_de_Tokens;
            }
            else
            {
                List<Token> Lista = new List<Token>();
                return Lista;
            }
        }

        /*
         * Metodo que imprime en HTML la lista de tokens encontrada en el texto de la
         * pestaña en focus
         */

        public void imprimirListaToken(string nombreArchivoER)
        {
            try
            {
                /*
                 * Creacion del html
                */
                String Lista_de_Tokens_HTML = "<html>" +
                    "<head>" +
                    "<meta charset='utf-8'>" +
                    "<title>\n" +
                    "		Reporte de Tokens\n" +
                    "	</title>\n" +
                    "	<link rel=\"stylesheet\" type=\"text/css\" href=\"css/bootstrap.min.css\">\n" +
                    "	<script type=\"text/javascript\" src=\"js/bootstrap.min.js\"></script>\n" +
                    "	<script type=\"text/javascript\" src=\"js/jquery-3.4.1.min.js\"></script>\n" +
                    "<Script Language=\"JavaScript\">\n" +
                    " function DameLaFechaHora() {\n" +
                    "var hora = new Date()\n" +
                    "var hrs = hora.getHours();\n" +
                    "var min = hora.getMinutes();\n" +
                    "var hoy = new Date();\n" +
                    "var m = new Array();\n" +
                    "var d = new Array()\n" +
                    "var an = hoy.getFullYear();\n" +
                    "m[0] = \"Enero\"; m[1] = \"Febrero\"; m[2] = \"Marzo\";\n" +
                    "m[3] = \"Abril\"; m[4] = \"Mayo\"; \n" +
                    "m[5] = \"Junio\";m[6] = \"Julio\"; m[7] = \"Agosto\"; m[8] = \"Septiembre\";\n" +
                    "m[9] = \"Octubre\"; m[10] = \"Noviembre\"; m[11] = \"Diciembre\";\n" +
                    "document.write(hrs + \":\" + min + \" (\");\n" +
                    " document.write(hoy.getDate());\n" +
                    "document.write(\" de \")\n;" +
                    "document.write(m[hoy.getMonth()])\n;" +
                    "document.write(\" del \" + an)\n; " +
                    "document.write(\")\");\n" +
                    "}</Script>\n" +
                    "</head><body>\n" +
                    "<div class=\"shadow-lg p-3 mb-5 rounded bg-dark text-white\">\n" +
                    "		<center><h1>Reporte de Tokens \n" +
                    "  <small class=\"text-muted bg-white\">Lista de Tokens</small></h1> <script>DameLaFechaHora();</script> " +
                    "<br>- Archivo HTML: " +
                    Application.StartupPath + "/Archivos HTMLS/Reporte de Tokens.html" +
                    "<br>- Archivo ER: " +
                    nombreArchivoER + " <br>" + "</center> \n" +
                    "</div>\n" +
                    "	<div class=\"container\">\n" +
                    "		<table class=\"table table-hover table-light text-center\">\n" +
                    " 			 <thead class=\"thead-dark\">   					 <tr>\n" +
                    "				      <th scope=\"col\">    #   </th>\n" +
                    "				      <th scope=\"col\">    LINEA    </th>\n" +
                    "				      <th scope=\"col\" style=\"width: 450px\">LEXEMA</th>\n" +
                    "				      <th scope=\"col\" style=\"width: 400px\">         TOKEN         </th>\n" +
                    "				    </tr>\n" +
                    "				  </thead><tbody>";

                /*
                 * Enlistado del vector de tokens encontrado en el analisis
                 */
                for (int i = 0; i < Lista_de_Tokens.Count; i++)
                {
                    if (Lista_de_Tokens[i] != null && Lista_de_Tokens[i].tipoToken != Token.Tipo.Ultimo)
                    {
                        Lista_de_Tokens_HTML = Lista_de_Tokens_HTML +
                            "<tr>\n"
                         + "				      <th scope=\"row\">" + (i + 1) + "</th>\n"
                         + "				      <td>" + Lista_de_Tokens[i].GetLinea + "</td>\n"
                         + "				      <td style=\"width: 450px\" >" + Lista_de_Tokens[i].GetValor + "</td>\n"
                         + "				      <td style=\"width: 400px\">" + Lista_de_Tokens[i].GetTipo + "</td>\n"
                         + "				    </tr>";
                    }
                }
                /*
                 * Finalizacion del archivo HTML
                 */
                Lista_de_Tokens_HTML = Lista_de_Tokens_HTML +
                    "</tbody>\n"
                                + "				</table>\n"
                                + "\n"
                                + "			</div>\n"
                                + "		\n"
                                + "</body>\n"
                                + "</html>";

                /*
                 * Proceso de guardado del archivo html
                 */
                File.WriteAllText(Application.StartupPath + "\\Archivos HTMLS\\Reporte de Tokens.html", Lista_de_Tokens_HTML);

            }
            catch { }

        }

        /*
         * Metodo que imprime en HTML la lista de errores encontrados en el texto de la
         * pestaña en focus
         */

        public void imprimirListaErrores(string nombreArchivoER)
        {
            try
            {
                /*
                 * Creacion del html
                */

                String Lista_de_Errores_HTML = "<html>" +
                    "<head>" +
                    "<meta charset='utf-8'>" +
                    "<title>\n" +
                    "		Reporte de Errores\n" +
                    "	</title>\n" +
                    "	<link rel=\"stylesheet\" type=\"text/css\" href=\"css/bootstrap.min.css\">\n" +
                    "	<script type=\"text/javascript\" src=\"js/bootstrap.min.js\"></script>\n" +
                    "	<script type=\"text/javascript\" src=\"js/jquery-3.4.1.min.js\"></script>" +
                    "<Script Language=\"JavaScript\">\n" +
                " function DameLaFechaHora() {\n" +
                "var hora = new Date()\n" +
                "var hrs = hora.getHours();\n" +
                "var min = hora.getMinutes();\n" +
                "var hoy = new Date();\n" +
                "var m = new Array();\n" +
                "var d = new Array()\n" +
                "var an = hoy.getFullYear();\n" +
                "m[0] = \"Enero\"; m[1] = \"Febrero\"; m[2] = \"Marzo\";\n" +
                "m[3] = \"Abril\"; m[4] = \"Mayo\"; \n" +
                "m[5] = \"Junio\";m[6] = \"Julio\"; m[7] = \"Agosto\"; m[8] = \"Septiembre\";\n" +
                "m[9] = \"Octubre\"; m[10] = \"Noviembre\"; m[11] = \"Diciembre\";\n" +
                "document.write(hrs + \":\" + min + \" (\");\n" +
                " document.write(hoy.getDate());\n" +
                "document.write(\" de \")\n;" +
                "document.write(m[hoy.getMonth()])\n;" +
                "document.write(\" del \" + an)\n; " +
                "document.write(\")\");\n" +
                "}</Script>\n" +
                    "</head><body>\n" +
                    "<div class=\"shadow-lg p-3 mb-5 rounded bg-dark text-white\">\n" +
                    "		<center><h1>Reporte de Errores \n" +
                    "  <small class=\"text-muted bg-white\">Lista de Tokens</small></h1> <script>DameLaFechaHora();</script>" +
                "<br>- Archivo HTML: " +
                Application.StartupPath + "/Archivos HTMLS/Reporte de Errores.html" +
                "<br>- Archivo ER: " +
                nombreArchivoER + " <br> </center> \n" +
                "</div>\n" +
                    "	<div class=\"container\">\n" +
                    "		<table class=\"table table-hover table-light text-center\">\n" +
                    " 			 <thead class=\"thead-dark\">   					 <tr>\n" +
                    "				      <th scope=\"col\">    #   </th>\n" +
                    "				      <th scope=\"col\">   FILA  </th>\n" +
                    "				      <th scope=\"col\">    COLUMNA    </th>\n" +
                    "				      <th scope=\"col\" style=\"width: 400px\">CARACTER</th>\n" +
                    "				      <th scope=\"col\" style=\"width: 450px\">         DESCRIPCION         </th>\n" +
                    "				    </tr>\n" +
                    "				  </thead><tbody>";

                /*
                 * Enlistado del vector de errrores encontrados en el analisis
                 */

                for (int i = 0; i < Lista_de_Errores.Count; i++)
                {
                    if (Lista_de_Errores[i] != null)
                    {
                        Lista_de_Errores_HTML = Lista_de_Errores_HTML +
                            "<tr>\n"
                         + "				      <th scope=\"row\">" + (i + 1) + "</th>\n"
                         + "				      <td>" + Lista_de_Errores[i].GetLinea + "</td>\n"
                         + "				      <td>" + Lista_de_Errores[i].GetColumna + "</td>\n"
                         + "				      <td style=\"width: 450px\" >" + Lista_de_Errores[i].GetValor + "</td>\n"
                         + "				      <td style=\"width: 400px\">" + Lista_de_Errores[i].GetTipo + "</td>\n"
                         + "				    </tr>";
                    }
                }

                /*
                 * Finalizacion del archivo HTML
                 */

                Lista_de_Errores_HTML = Lista_de_Errores_HTML +
                    "</tbody>\n"
                                + "				</table>\n"
                                + "\n"
                                + "			</div>\n"
                                + "		\n"
                                + "</body>\n"
                                + "</html>";

                /*
                 * Proceso de guardado del archivo html
                 */

                File.WriteAllText(Application.StartupPath + "\\Archivos HTMLS\\Reporte de Errores.html", Lista_de_Errores_HTML);


            }
            catch { }
        }


        public void abrirHTMLTokens()
        {
            /*
            *Proceso de inicir el archivo html generado 
            */
            Process.Start(Application.StartupPath + "\\Archivos HTMLS\\Reporte de Tokens.html");
        }

        public void abrirHTMLErrores()
        {
            /*
             *Proceso de inicir el archivo html generado 
             */
            Process.Start(Application.StartupPath + "\\Archivos HTMLS\\Reporte de Errores.html");
        }
    }
}

