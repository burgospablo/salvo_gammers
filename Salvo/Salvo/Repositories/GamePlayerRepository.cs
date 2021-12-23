using Microsoft.EntityFrameworkCore;
using Salvo.Models;
using System.Linq;

namespace Salvo.Repositories
{
    public class GamePlayerRepository : RepositoryBase<GamePlayer>, IGamePlayerRepository
    {
        public GamePlayerRepository(SalvoContext repositoryContext) : base(repositoryContext)
        {
        }

        /*-----------------------------Actividad 10------------------------------*/
        public GamePlayer FindById(long id)
        {
            //trae los datos directo de la BD 
            return FindByCondition(gp => gp.Id == id)
                 .Include(gp => gp.Game)
                        .ThenInclude(game => game.GamePlayers)
                            .ThenInclude(gp => gp.Salvos)
                 .Include(gp => gp.Game)
                        .ThenInclude(game => game.GamePlayers)
                            .ThenInclude(gp => gp.Ships)
                 .Include(gp => gp.Player)
                 .Include(gp => gp.Ships)
                 .Include(gp => gp.Salvos)
                 .FirstOrDefault();
        }

        //public GamePlayer FindById(long id)
        //{
        //    //trae los datos directo de la BD 
        //    return FindByCondition(gp => gp.Id == id)
        //         .Include(gp => gp.Player)
        //         .Include(gp => gp.Ships)
        //         .FirstOrDefault();
        //}

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

        public void Save(GamePlayer gamePlayer)
        {
            if (gamePlayer.Id == 0)
                Create(gamePlayer);
            else
                Update(gamePlayer);
            SaveChanges();
        }
    }
}
