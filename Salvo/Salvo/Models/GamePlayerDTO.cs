using System;

namespace Salvo.Models
{
    public class GamePlayerDTO
    {
        public long Id { get; set; }
        public DateTime? JoinDate { get; set; }
        public PlayerDTO Player { get; set; }
        public double? Point { get; set; }//el valor puede ser nulo dado que el juego no ha terminado

    }
}
