using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PlantillaSpa.Api.ParametrosJavascript
{
    public class GuardarNombreDeusuario
    {
        [JsonPropertyName("sUsuario")]
        public string NombreUsuario { get; set; }
    }
}
