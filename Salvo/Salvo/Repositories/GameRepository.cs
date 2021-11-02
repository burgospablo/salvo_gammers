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

       
    }
}
