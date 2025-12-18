using System;

namespace Arandata.Domain.Entities
{
    public class ArticuloBaja
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string? Reason { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
