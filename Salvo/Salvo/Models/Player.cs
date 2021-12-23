using System.Collections.Generic;
using System.Linq;

namespace Salvo.Models
{
    public class Player
    {
        //1° Defino el Id del tipo long
        public long Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public ICollection<GamePlayer> GamePlayers { get; set; }
        public ICollection<Score> Scores { get; set; }

        //Tomo de la lista de arriba Scores y el elemento que corresponda con el game que pregunta
        //por parametro para retornar
        public Score GetScore(Game game)
        {
            //retorna de los scores el primero o el por defecto que cumpla la condicion
            //FirstOrDefault: devuelve el primero o el por defecto o con una condicion
            //el predicado es score y del score necesito el GameId y que sea igual al game que viene por 
            //parametro punto Id
            return Scores.FirstOrDefault(score => score.GameId == game.Id);
        }

    }
}
