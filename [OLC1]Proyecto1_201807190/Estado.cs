using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_Proyecto1_201807190
{
    class Estado
    {
        /*
         * Atributos del objeto
         */
        private int id;
        private List<Transicion> transiciones = new List<Transicion>();

        /*
         * Constructor del objeto Estado, que recibe como parametro un identificador
         */
        public Estado(int id)
        {
            this.Id = id;
        }

        /*
         * Constructor del objeto Estado, que recibe como parametro el 
         * identificador y una lista de transiciones
         */
        public Estado(int id, List<Transicion> transiciones) : this(id)
        {
            this.Transiciones = transiciones;
        }

        /*
        * Accesores y modificadores de todos los atributos
        */
        public int Id { get => id; set => id = value; }
        public List<Transicion> Transiciones { get => transiciones; set => transiciones = value; }

        override
        public String ToString()
        {
            return this.id.ToString();
        }
    }
}
