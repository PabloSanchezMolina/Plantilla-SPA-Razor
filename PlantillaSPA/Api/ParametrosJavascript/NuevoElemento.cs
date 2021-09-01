using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PlantillaSpa.Api.ParametrosJavascript
{
    public class NuevoElemento
    {
        [JsonPropertyName("sElemento")]
        public string NombreElemento { get; set; }
    }
}
