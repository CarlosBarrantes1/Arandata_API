using System;
using System.Collections.Generic;

namespace Arandata.Domain.Entities
{
    public class Cosecha
    {
        public int Id { get; set; }
        public int LoteId { get; set; }
        public Lote? Lote { get; set; }
        public DateTime FechaCosecha { get; set; }
        public decimal? KgTotal { get; set; }
        public decimal? KgTotalAcumulado { get; set; }
        public decimal? KgPlanta { get; set; }
        public decimal? KgPlantaAcumulado { get; set; }
        public int? DiasDespuesPoda { get; set; }

        // Relación con Muestras
        public ICollection<Muestra> Muestras { get; set; } = new List<Muestra>();
    }
}
