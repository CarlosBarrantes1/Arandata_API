using System;

namespace Arandata.Domain.Entities
{
    public class Poda
    {
        public int Id { get; set; }
        public int LoteId { get; set; }
        public Lote? Lote { get; set; }
        public DateTime FechaPoda { get; set; }
    }
}
