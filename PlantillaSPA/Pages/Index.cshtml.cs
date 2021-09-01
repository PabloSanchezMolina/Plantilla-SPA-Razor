using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using PlantillaSpa.DAO;
using PlantillaSpa.DTO;

namespace PlantillaSpa.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppSettings _appSettings;
        public PrimeraCarga PrimeraCarga { get; set; }

        private int FK_Usuario
        {
            get
            {
                //TODO: implementar autenticación de usuario y devolver Convert.ToInt32(_userManager.GetUserId(HttpContext.User));
                return 1;
            }
        }

        public IndexModel(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public async Task OnGetAsync()
        {
            try
            {
                Actividad actividad = await Actividad.Get(FK_Usuario, _appSettings.Redis);
                if (actividad == null)
                {
                    //No hay estado almacenado para nuestro usuario. Inicializamos los parámetros obligatorios para los nuevos usuarios:
                    actividad = new Actividad
                    {
                        Actual = Actividad.Estado.Inicio,
                        FK_Usuario = FK_Usuario,
                        UsuarioNombre = String.Empty,
                        Listas = new List<DAO.Lista>()
                    };
                    await actividad.Persistir(_appSettings.Redis);
                }

                PrimeraCarga = new PrimeraCarga
                {
                    Actual = actividad.Actual
                };
                //Recuperamos y almacenamos en PrimeraCarga únicamente el modelo que necesitará nuestra página de Razor para pintar según el estado actual de la aplicación:
                switch (actividad.Actual)
                {
                    case Actividad.Estado.Inicio:
                        PrimeraCarga.Inicio = await actividad.GetInicio();
                        break;
                    case Actividad.Estado.EditarLista:
                        PrimeraCarga.EditarLista = await actividad.GetEditarLista();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            catch (Exception e)
            {
                //TODO: manejar errores
            }
        }
    }
}
