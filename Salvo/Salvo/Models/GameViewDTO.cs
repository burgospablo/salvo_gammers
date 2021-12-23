using System;
using System.Collections.Generic;

namespace Salvo.Models
{
    public class GameViewDTO
    {
        public long Id { get; set; }
        public DateTime? CreationDate { get; set; }
        public ICollection<GamePlayerDTO> GamePlayers { get; set; }
        public ICollection<ShipDTO> Ships { get; set; }
        public ICollection<SalvoDTO> Salvos { get; set; }

        /*----------------------Actividad 10---------------------------------*/
        public ICollection<SalvoHitDTO> Hits { get; set; }
        public ICollection<SalvoHitDTO> HitsOpponent { get; set; }
        public ICollection<string> Sunks { get; set; }
        public ICollection<string> SunksOpponent { get; set; }

        /*----------------------Actividad 11---------------------------------*/
        public string GameState { get; set; }
    }
}
