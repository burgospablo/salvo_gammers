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

        }
    }
}
