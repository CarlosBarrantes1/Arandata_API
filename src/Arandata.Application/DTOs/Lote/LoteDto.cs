
using System;
using System.Text.Json.Serialization;

namespace Arandata.Application.DTOs.Lote
{
    public class LoteDto
    {
        public int idLote { get; set; }
        public string nombre { get; set; } = string.Empty;
        public int variedadId { get; set; }
        public DateTime? fechaSiembra { get; set; }
        public DateTime? fechaPoda { get; set; }
        public int? plantasTotales { get; set; }
        public decimal? hectareas { get; set; }
    }
}
