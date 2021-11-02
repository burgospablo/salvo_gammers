﻿using Salvo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Salvo.Repositories
{
    public interface IGameRepository
    {
        IEnumerable<Game> GetAllGames();//Trae todos los juegos
       // IEnumerable<Game> GetAllGamesWithPlayers();//Trae todos los juegos con jugadores
    }
}