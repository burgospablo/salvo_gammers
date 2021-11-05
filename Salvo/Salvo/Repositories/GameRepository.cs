using Microsoft.EntityFrameworkCore;
using Salvo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Salvo.Repositories
{
    //Esta clase hereda de RepositoryBase<Game> y ademas implementa la interfaz IGameRepository
    public class GameRepository : RepositoryBase<Game>, IGameRepository
    {
        public GameRepository(SalvoContext repositoryContext) : base(repositoryContext)
        {
           // this.GameRepository = repositoryContext;
        }

        public IEnumerable<Game> GetAllGames()
        {
            //Retorno todos los elementos de game de forma ordenada mediante 
            //su fecha de creacion y que sea una lista de games
            return FindAll().OrderBy(game=>game.CreationDate).ToList();
        }


        //Cuando necesito incluir informacion relacionada a las entidades que debo ocupar
        //dentro de los controladores, el lugar para incluir esa informacion es aqui en
        //el repositorio, no en el controlador.En el controlador transformo los datos. 
        public IEnumerable<Game> GetAllGamesWithPlayers()
        {
            //En el metodo FindAll que recibe los includes, le digo que de mi game incluya
            //GamePlayers, pero ademas de los gameplayer incluye el Player
            //Ademas los ordena por fecha de creacion y que todo lo retorne como una lista
            return FindAll(source => source.Include(game => game.GamePlayers)
                    .ThenInclude(gameplayer => gameplayer.Player))
                .OrderBy(game => game.CreationDate)
                .ToList();
        }

        //public IEnumerable<GameDTO> GetAllGamesWithPlayers()
        //{
        //    return FindAll(source => source.Include(game => game.GamePlayers)
        //                .ThenInclude(gameplayer => gameplayer.Player))
        //            .OrderBy(game => game.CreationDate)
        //                .Select(g => new GameDTO
        //                {
        //                    Id = g.Id,
        //                    CreationDate = g.CreationDate,
        //                    GamePlayers = g.GamePlayers.Select(gp => new GamePlayerDTO { Id = gp.Id, JoinDate = gp.JoinDate, Player = new PlayerDTO { Id = gp.Player.Id, Email = gp.Player.Email } }).ToList()
        //                })
        //            .ToList();
        //}


    }
}
