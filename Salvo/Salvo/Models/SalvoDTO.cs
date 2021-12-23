using System.Collections.Generic;

namespace Salvo.Models
{
    public class SalvoDTO
    {
        public long Id { get; set; }
        public int Turn { get; set; }
        public PlayerDTO Player { get; set; }
        public ICollection<SalvoLocationDTO> Locations { get; set; }
    }
}
