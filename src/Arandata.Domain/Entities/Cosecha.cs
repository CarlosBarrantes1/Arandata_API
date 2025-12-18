using System;

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

        public ICollection<Muestra100> Muestras100 { get; set; } = new List<Muestra100>();
        public ICollection<MuestraBrix> MuestrasBrix { get; set; } = new List<MuestraBrix>();
    }
}
