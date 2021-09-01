using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlantillaSpa.DTO
{
    public class EditarLista
    {
        public string Nombre { get; set; }
        public List<Elemento> Elementos { get; set; }
    }
}
