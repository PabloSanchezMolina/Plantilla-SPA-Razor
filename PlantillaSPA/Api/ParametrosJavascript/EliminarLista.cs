﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PlantillaSpa.Api.ParametrosJavascript
{
    public class EliminarLista
    {
        [JsonPropertyName("iId")]
        public int IdLista { get; set; }
    }
}
