using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Salvo.Models
{
    public static class DbInitializer
    {
        //Metodo estatico que no devuelve nada solo hace lo que le digo y nada mas.
        public static void Initialize(SalvoContext context)
        {

            // Chequeo si hay jugadores, si no hay agrego jugadores
            if (!context.Players.Any()) // sino existe alguno si quiera agregalo o forma seria: context.Players.Any() == false
            {
                var players = new Player[]
                {
                new Player { Email = "j.bauer@ctu.gov", Name = "Jack Bauer", Password = "24" },
                new Player { Email = "c.obrian@ctu.gov", Name = "Chloe O'Brian", Password = "42" },
                new Player { Email = "kim_bauer@gmail.com", Name = "Kim Bauer", Password = "kb" },
                new Player { Email = "t.almeida@ctu.gov", Name = "Tony Almeida", Password = "mole" },
                };
                //recorre los players
                foreach (Player p in players)
                {
                    context.Players.Add(p);
                }
                //guardo los cambios
                context.SaveChanges();
            }

            // Chequeo si hay juegos, si no hay agrego juegos
            if (!context.Games.Any()) // sino existe alguno si quiera agregalo o de otra forma seria: context.Games.Any() == false
            {
                var games = new Game[]
                {
                //Inicializo con fecha y hora actual
                new Game { CreationDate = DateTime.Now},
                //Agrego 1 hora al anterior
                new Game { CreationDate = DateTime.Now.AddHours(1) },
                new Game { CreationDate = DateTime.Now.AddHours(2) },
                new Game { CreationDate = DateTime.Now.AddHours(3) },
                new Game { CreationDate = DateTime.Now.AddHours(4) },
                new Game { CreationDate = DateTime.Now.AddHours(5) },
                new Game { CreationDate = DateTime.Now.AddHours(6) },
                new Game { CreationDate = DateTime.Now.AddHours(7) },
                };
                //recorre los games
                foreach (Game g in games)
                {
                    context.Games.Add(g);
                }
                //guardo los cambios
                context.SaveChanges();
            }


        }
    }
}
