using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PlantillaSpa.Api
{
    public class Resultado
    {
        [JsonPropertyName("sHtml")]
        public string Html { get; set; }
        [JsonPropertyName("bCorrecto")]
        public bool Correcto { get; set; }
        [JsonPropertyName("sMotivo")]
        public string Motivo { get; set; }
    }
}
