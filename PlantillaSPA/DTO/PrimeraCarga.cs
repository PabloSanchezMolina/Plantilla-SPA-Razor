using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlantillaSpa.DAO;

namespace PlantillaSpa.DTO
{
    public class PrimeraCarga
    {
        public Actividad.Estado Actual { get; set; }
        public Inicio Inicio { get; set; }
        public EditarLista EditarLista { get; set; }
    }
}
