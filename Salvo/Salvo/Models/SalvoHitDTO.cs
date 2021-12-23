using System.Collections.Generic;

/*-----------------------------------Actividad 10--------------------------*/
namespace Salvo.Models
{
    public class SalvoHitDTO
    {
        public int Turn { get; set; }
        public List<ShipHitDTO> Hits { get; set; }
    }
}
