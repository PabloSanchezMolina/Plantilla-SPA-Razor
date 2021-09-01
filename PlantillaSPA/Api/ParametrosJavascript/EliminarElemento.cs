using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PlantillaSpa.Api.ParametrosJavascript
{
    public class EliminarElemento
    {
        [JsonPropertyName("iElemento")]
        public int IdElemento { get; set; }
    }
}
