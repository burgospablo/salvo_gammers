using Microsoft.EntityFrameworkCore;
using Salvo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Salvo.Repositories
{
    public class GamePlayerRepository : RepositoryBase<GamePlayer>, IGamePlayerRepository
    {
        public GamePlayerRepository(SalvoContext repositoryContext) : base(repositoryContext)
        {
        }

        //public GamePlayer GetGamePlayerView(long idGamePlayer)
        //{
        //    return FindAll(source => source.Include(gamePlayer => gamePlayer.Ships)//de gameplayer incluye los ships
        //                                        .ThenInclude(ship => ship.Locations)//de los ship incluyo Locations
        //                                   .Include(gamePlayer => gamePlayer.Game)//de gameplayer incluye los game
        //                                        .ThenInclude(game => game.GamePlayers)//de los ship incluyo gameplayers
        //                                            .ThenInclude(gp => gp.Player)//incluye los players
        //                                    )
        //        .Where(gamePlayer => gamePlayer.Id == idGamePlayer)//bajo la condicion de que gameplayer.id sea igual al id que viene del parametro
        //        .OrderBy(game => game.JoinDate)//ordeno por game a traves de joindate
        //        .FirstOrDefault();//retorno el primero o el por defecto
        //}

        public GamePlayer GetGamePlayerView(long idGamePlayer)
        {
            return FindAll(source => source.Include(gamePlayer => gamePlayer.Ships)
                                                .ThenInclude(ship => ship.Locations)
                                            .Include(gamePlayer => gamePlayer.Salvos)
                                                .ThenInclude(salvo => salvo.Locations)
                                            .Include(gamePlayer => gamePlayer.Game)
                                                .ThenInclude(game => game.GamePlayers)
                                                    .ThenInclude(gp => gp.Player)
                                            .Include(gamePlayer => gamePlayer.Game)
                                                .ThenInclude(game => game.GamePlayers)
                                                    .ThenInclude(gp => gp.Salvos)
                                                        .ThenInclude(salvo => salvo.Locations)
                                            .Include(gamePlayer => gamePlayer.Game)
                                                .ThenInclude(game => game.GamePlayers)
                                                    .ThenInclude(gp => gp.Ships)
                                                        .ThenInclude(ship => ship.Locations)
                                            )
                .Where(gamePlayer => gamePlayer.Id == idGamePlayer)
                .OrderBy(game => game.JoinDate)
                .FirstOrDefault();
        }
    }
}
