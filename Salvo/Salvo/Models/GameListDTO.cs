using System.Collections.Generic;

namespace Salvo.Models
{
    public class GameListDTO
    {
        public string Email { get; set; }
        public ICollection<GameDTO> Games { get; set; }
    }
}
