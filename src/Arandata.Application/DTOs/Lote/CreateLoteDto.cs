using System;

namespace Arandata.Application.DTOs.Lote
{
    public class CreateLoteDto
    {
        [System.Text.Json.Serialization.JsonPropertyName("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [System.Text.Json.Serialization.JsonPropertyName("variedadId")]
        public int VariedadId { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("fechaSiembra")]
        public DateTime? FechaSiembra { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("fechaPoda")]
        public DateTime? FechaPoda { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("plantasTotales")]
        public int? PlantasTotales { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("hectareas")]
        public decimal? Hectareas { get; set; }
    }
}
