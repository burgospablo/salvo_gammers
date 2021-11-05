﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Salvo.Models
{
    public class SalvoContext : DbContext
    {
        //Constructor
        public SalvoContext(DbContextOptions<SalvoContext> options):base(options)
        {
        }
        //Agrego al contexto el modelo que seria: la clase Player
        public DbSet<Player> Players { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GamePlayer> GamePlayers { get; set; }
       
    }
}
