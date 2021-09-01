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
        public string UsuarioNombre { get; set; }
        public List<Lista> Listas { get; set; }
        //Estas son las propiedades con la información del progreso de nuestro usuario en su máquina de estado que almacenaremos en Redis.fin

        public async Task<Inicio> GetInicio()
        {
            return new Inicio()
            {
                Nombre = UsuarioNombre,
                Listas = Listas.Select(x => new DTO.Lista
                {
                    Id = x.Id,
                    Nombre = x.Nombre
                }).ToList()
            };
        }
    }
}
