using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Salvo.Models
{
    public class Game
    {
        public long Id { get; set; }
        //Agrego el signo de preguntas para que c# interprete que acepta valores nulos
        public DateTime? CreationDate { get; set; }
        public ICollection<GamePlayer> GamePlayers { get; set; }
    }
}
