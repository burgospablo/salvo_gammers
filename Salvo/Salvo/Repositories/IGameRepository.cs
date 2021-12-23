using Salvo.Models;
using System.Collections.Generic;

namespace Salvo.Repositories
{
    public interface IGameRepository
    {
        IEnumerable<Game> GetAllGames();//Trae todos los juegos
        IEnumerable<Game> GetAllGamesWithPlayers();//Trae todos los juegos con jugadores
        Game FindById(long id);

    }
}
