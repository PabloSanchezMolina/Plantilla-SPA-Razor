using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace PlantillaSpa.Api
{
    public static class Renderizado
    {
        public static async Task<string> GenerarHtml<TModel>(this Controller controlador, string nombreDeLaVista, TModel model, bool esVistaParcial = false)
        {
            if (string.IsNullOrEmpty(nombreDeLaVista))
            {
                nombreDeLaVista = controlador.ControllerContext.ActionDescriptor.ActionName;
            }

            controlador.ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                IViewEngine viewEngine = controlador.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                ViewEngineResult viewResult = viewEngine.FindView(controlador.ControllerContext, nombreDeLaVista, !esVistaParcial);

                if (viewResult.Success == false)
                {
                    return $"La vista con el nombre {nombreDeLaVista} no existe";
                }

                ViewContext viewContext = new ViewContext(
                    controlador.ControllerContext,
                    viewResult.View,
                    controlador.ViewData,
                    controlador.TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
        }
    }
}
