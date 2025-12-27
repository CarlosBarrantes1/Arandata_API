using System;

namespace Arandata.Domain.Entities
{
    public class Lote
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int VariedadId { get; set; }
        public Variedad? Variedad { get; set; }
        public DateTime? FechaSiembra { get; set; }
        public int? PlantasTotales { get; set; }
        public decimal? Hectareas { get; set; }

        public ICollection<Cosecha> Cosechas { get; set; } = new List<Cosecha>();
        public ICollection<Poda> Podas { get; set; } = new List<Poda>();
    }
}
