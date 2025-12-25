using System;
using System.Collections.Generic;

namespace Arandata.Domain.Entities
{
    public class Muestra
    {
        public int Id { get; set; }
        public int CosechaId { get; set; }
        public DateTime FechaMuestreo { get; set; }

        public Cosecha? Cosecha { get; set; }
        public ICollection<BayaDiametro> BayasDiametro { get; set; } = new List<BayaDiametro>();
        public ICollection<BayaPeso> BayasPeso { get; set; } = new List<BayaPeso>();
        public ICollection<BayaBrix> BayasBrix { get; set; } = new List<BayaBrix>();
        public ICollection<MuestraTipo> MuestraTipos { get; set; } = new List<MuestraTipo>();
    }
}
