using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_Proyecto1_201807190
{
    class TokenLexema
    {
        private Token token;
        private string expresion;

        public TokenLexema(Token token, string expresion)
        {
            this.Token = token;
            this.Expresion = expresion;
        }

        public string Expresion { get => expresion; set => expresion = value; }
        public Token Token { get => token; set => token = value; }
    }
}
