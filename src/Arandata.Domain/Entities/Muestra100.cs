using System;

namespace Arandata.Domain.Entities
{
    public class Muestra100
    {
        public int Id { get; set; }
        public int CosechaId { get; set; }
        public Cosecha? Cosecha { get; set; }
        public DateTime FechaMuestreo { get; set; }

        public ICollection<Baya100> Bayas { get; set; } = new List<Baya100>();
    }
}
