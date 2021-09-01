using PlantillaSpa.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlantillaSpa.DAO
{
    public partial class Actividad
    {
        //Estas son las propiedades con la información del progreso de nuestro usuario en su máquina de estado que almacenaremos en Redis.ini
        public int ListaIdSeleccionada { get; set; }
        //Estas son las propiedades con la información del progreso de nuestro usuario en su máquina de estado que almacenaremos en Redis.fin

        public async Task<EditarLista> GetEditarLista()
        {
            Lista listaSeleccionada = Listas.Find(x => x.Id == ListaIdSeleccionada);

            return new EditarLista()
            {
                Nombre = listaSeleccionada.Nombre,
                Elementos = listaSeleccionada.Elementos.Select(x => new DTO.Elemento
                {
                    Id = x.Id,
                    Nombre = x.Nombre
                }).ToList()
            };
        }
    }
}
