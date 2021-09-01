using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlantillaSpa.DAO
{
    public class Lista
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<Elemento> Elementos { get; set; }
    }
    public class Elemento
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
}
