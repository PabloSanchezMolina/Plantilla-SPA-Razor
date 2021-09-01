using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PlantillaSpa.DAO;
using PlantillaSpa.DTO;
using Lista = PlantillaSpa.DAO.Lista;

namespace PlantillaSpa.Api
{
    [Route("api")]
    [ValidateAntiForgeryToken]
    public class AjaxController : Controller
    {
        private readonly AppSettings _appSettings;
        private int FK_Usuario
        {
            get
            {
                //TODO: implementar autenticación de usuario y devolver Convert.ToInt32(_userManager.GetUserId(HttpContext.User));
                return 1;
            }
        }

        public AjaxController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        [HttpPost]
        [Route("guardar-nombre-de-usuario")]
        public async Task<JsonResult> GuardarNombreDeusuario([FromBody] ParametrosJavascript.GuardarNombreDeusuario datos)
        {
            Resultado resultado = new Resultado();
            try
            {
                //Recuperamos la actividad del usuario
                Actividad actividad = await Actividad.Get(FK_Usuario, _appSettings.Redis);
                if (actividad != null)
                {
                    actividad.UsuarioNombre = datos.NombreUsuario;
                    await actividad.Persistir(_appSettings.Redis);
                    
                    Inicio inicio = await actividad.GetInicio();

                    resultado.Correcto = true;
                    resultado.Html = await Renderizado.GenerarHtml(this, "../Inicio/Index", inicio, true);
                }
                else
                {
                    throw new Exception("Este usuario no está registrado en el sistema");
                }
            }
            catch (Exception e)
            {
                resultado.Correcto = false;
                resultado.Motivo = e.Message;
            }
            return new JsonResult(resultado);
        }

        [HttpPost]
        [Route("crear-lista")]
        public async Task<JsonResult> CrearLista([FromBody] ParametrosJavascript.CrearLista datos)
        {
            Resultado resultado = new Resultado();
            try
            {
                //Recuperamos la actividad del usuario
                Actividad actividad = await Actividad.Get(FK_Usuario, _appSettings.Redis);
                if (actividad != null)
                {
                    if (actividad.Listas.Exists(x => x.Nombre == datos.NombreLista))
                    {
                        throw new Exception("Ya existe una lista con ese nombre");
                    }
                    else
                    {
                        actividad.Listas.Add(new DAO.Lista
                        {
                            Id = actividad.Listas.Count + 1,
                            Nombre = datos.NombreLista,
                            Elementos = new List<DAO.Elemento>()
                        });
                        await actividad.Persistir(_appSettings.Redis);

                        Inicio inicio = await actividad.GetInicio();

                        resultado.Correcto = true;
                        resultado.Html = await Renderizado.GenerarHtml(this, "../Inicio/Index", inicio, true);
                    }
                }
                else
                {
                    throw new Exception("Este usuario no está registrado en el sistema");
                }
            }
            catch (Exception e)
            {
                resultado.Correcto = false;
                resultado.Motivo = e.Message;
            }
            return new JsonResult(resultado);
        }

        [HttpPost]
        [Route("eliminar-lista")]
        public async Task<JsonResult> EliminarLista([FromBody] ParametrosJavascript.EliminarLista datos)
        {
            Resultado resultado = new Resultado();
            try
            {
                //Recuperamos la actividad del usuario
                Actividad actividad = await Actividad.Get(FK_Usuario, _appSettings.Redis);
                if (actividad != null)
                {
                    actividad.Listas.RemoveAll(x => x.Id == datos.IdLista);
                    await actividad.Persistir(_appSettings.Redis);

                    Inicio inicio = await actividad.GetInicio();

                    resultado.Correcto = true;
                    resultado.Html = await Renderizado.GenerarHtml(this, "../Inicio/Index", inicio, true);
                }
                else
                {
                    throw new Exception("Este usuario no está registrado en el sistema");
                }
            }
            catch (Exception e)
            {
                resultado.Correcto = false;
                resultado.Motivo = e.Message;
            }
            return new JsonResult(resultado);
        }

        [HttpPost]
        [Route("editar-lista")]
        public async Task<JsonResult> EditarLista([FromBody] ParametrosJavascript.EditarLista datos)
        {
            Resultado resultado = new Resultado();
            try
            {
                //Recuperamos la actividad del usuario
                Actividad actividad = await Actividad.Get(FK_Usuario, _appSettings.Redis);
                if (actividad != null)
                {
                    actividad.Actual = Actividad.Estado.EditarLista;
                    actividad.ListaIdSeleccionada = datos.IdLista;
                    await actividad.Persistir(_appSettings.Redis);

                    EditarLista editarLista = await actividad.GetEditarLista();

                    resultado.Correcto = true;
                    resultado.Html = await Renderizado.GenerarHtml(this, "../EditarLista/Index", editarLista, true);
                }
                else
                {
                    throw new Exception("Este usuario no está registrado en el sistema");
                }
            }
            catch (Exception e)
            {
                resultado.Correcto = false;
                resultado.Motivo = e.Message;
            }
            return new JsonResult(resultado);
        }

        [HttpPost]
        [Route("nuevo-elemento")]
        public async Task<JsonResult> NuevoElemento([FromBody] ParametrosJavascript.NuevoElemento datos)
        {
            Resultado resultado = new Resultado();
            try
            {
                //Recuperamos la actividad del usuario
                Actividad actividad = await Actividad.Get(FK_Usuario, _appSettings.Redis);
                if (actividad != null)
                {
                    Lista listaSeleccionada = actividad.Listas.FirstOrDefault(x => x.Id == actividad.ListaIdSeleccionada);
                    if (listaSeleccionada != null)
                    {
                        if (listaSeleccionada.Elementos.Exists(x => x.Nombre == datos.NombreElemento))
                        {
                            throw new Exception("Ya existe un elemento con ese nombre en esta lista");
                        }
                        else
                        {
                            listaSeleccionada.Elementos.Add(new DAO.Elemento
                            {
                                Id = listaSeleccionada.Elementos.Count + 1,
                                Nombre = datos.NombreElemento
                            });
                            await actividad.Persistir(_appSettings.Redis);

                            EditarLista editarLista = await actividad.GetEditarLista();

                            resultado.Correcto = true;
                            resultado.Html = await Renderizado.GenerarHtml(this, "../EditarLista/Index", editarLista, true);
                        }
                    }
                    else
                    {
                        throw new Exception("Lista no encontrada");
                    }
                }
                else
                {
                    throw new Exception("Este usuario no está registrado en el sistema");
                }
            }
            catch (Exception e)
            {
                resultado.Correcto = false;
                resultado.Motivo = e.Message;
            }
            return new JsonResult(resultado);
        }

        [HttpPost]
        [Route("eliminar-elemento")]
        public async Task<JsonResult> EliminarElemento([FromBody] ParametrosJavascript.EliminarElemento datos)
        {
            Resultado resultado = new Resultado();
            try
            {
                //Recuperamos la actividad del usuario
                Actividad actividad = await Actividad.Get(FK_Usuario, _appSettings.Redis);
                if (actividad != null)
                {
                    Lista listaSeleccionada = actividad.Listas.FirstOrDefault(x => x.Id == actividad.ListaIdSeleccionada);
                    if (listaSeleccionada != null)
                    {
                        listaSeleccionada.Elementos.RemoveAll(x => x.Id == datos.IdElemento);
                        await actividad.Persistir(_appSettings.Redis);

                        EditarLista editarLista = await actividad.GetEditarLista();

                        resultado.Correcto = true;
                        resultado.Html = await Renderizado.GenerarHtml(this, "../EditarLista/Index", editarLista, true);
                    }
                    else
                    {
                        throw new Exception("Lista no encontrada");
                    }
                }
                else
                {
                    throw new Exception("Este usuario no está registrado en el sistema");
                }
            }
            catch (Exception e)
            {
                resultado.Correcto = false;
                resultado.Motivo = e.Message;
            }
            return new JsonResult(resultado);
        }

        [HttpPost]
        [Route("volver-a-inicio")]
        public async Task<JsonResult> VolverAInicio()
        {
            Resultado resultado = new Resultado();
            try
            {
                //Recuperamos la actividad del usuario
                Actividad actividad = await Actividad.Get(FK_Usuario, _appSettings.Redis);
                if (actividad != null)
                {
                    actividad.Actual = Actividad.Estado.Inicio;
                    await actividad.Persistir(_appSettings.Redis);

                    Inicio inicio = await actividad.GetInicio();

                    resultado.Correcto = true;
                    resultado.Html = await Renderizado.GenerarHtml(this, "../Inicio/Index", inicio, true);
                }
                else
                {
                    throw new Exception("Este usuario no está registrado en el sistema");
                }
            }
            catch (Exception e)
            {
                resultado.Correcto = false;
                resultado.Motivo = e.Message;
            }
            return new JsonResult(resultado);
        }
    }
}

