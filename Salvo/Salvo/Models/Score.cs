using System;

namespace Salvo.Models
{
    public class Score
    {
        public long Id { get; set; }
        public double Point { get; set; }//0:score asociado al player perdio-0,5:empato-1:gano la partida
        public DateTime? FinishDate { get; set; }
        public long PlayerId { get; set; }
        public Player Player { get; set; }
        public long GameId { get; set; }
        public Game Game { get; set; }
    }
}
