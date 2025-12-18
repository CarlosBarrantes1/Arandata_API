using System;

namespace Arandata.Domain.Entities
{
    public class MuestraBrix
    {
        public int Id { get; set; }
        public int CosechaId { get; set; }
        public Cosecha? Cosecha { get; set; }
        public DateTime FechaMuestreo { get; set; }

        public ICollection<BayaBrix> Bayas { get; set; } = new List<BayaBrix>();
    }
}
