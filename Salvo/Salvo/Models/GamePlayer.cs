using System;
using System.Collections.Generic;
using System.Linq;

namespace Salvo.Models
{
    public class GamePlayer
    {
        public long Id { get; set; }
        public DateTime? JoinDate { get; set; }
        public long PlayerId { get; set; }
        public Player Player { get; set; }
        public long GameId { get; set; }
        public Game Game { get; set; }
        public ICollection<Ship> Ships { get; set; }
        public ICollection<Salvo> Salvos { get; set; }

        //Metodo que obtiene el score
        public Score GetScore()
        {
            //Retorno del Player que tiene el metodo GetScore y que por paramentro recibo un game
            // le voy a pasar el Game de Game o sea public Game Game { get; set; }
            return Player.GetScore(Game);
        }

        //Metodo para obtener el oponente, es decir distinto al que estaba autenticado
        public GamePlayer GetOpponet()
        {
            //Si en este momento hay un gameplayers distinto del mio 
            //Me trae el primero o el por defecto que sea distinto al que esta logueado(o sea yo)
            //entonces quiere decir que es el oponente
            return Game.GamePlayers.FirstOrDefault(gp => gp.Id != Id);
        }
        /*-----------------------------------Actividad 10--------------------------*/
        //Debo modificar para agregar los hits y los sunks:
        //para hacer esto debo tener una coleccion de hits que deberian ser del tipo salvohitsdto
        //y este salvohitsdto debe tener el turno y ships que lo llamo shiphitsdto el mismo tendra un
        //tipo y una lista de string que corresponde a los hits. Debo modificar los gameplayers para
        //agregar estos hits y sunks, pero debo agregar los dtos de: salvohitsdto y shiphitsdto.
        //Creo en models estas dos clases

        public ICollection<SalvoHitDTO> getHits()
        {
            return Salvos.Select(salvo => new SalvoHitDTO
            {
                Turn = salvo.Turn,
                Hits = GetOpponet()?.Ships.Select(ship => new ShipHitDTO
                {
                    Type = ship.Type,
                    //Trae todo los Locations de esos salvos y le llamo Hits, cuando los Location
                    //de los ship, cualquiera de ellos cumpla la condicion de que cuando: los Locations
                    //de los shipLocation sean iguales a los Locations de los salvoLocation
                    Hits = salvo.Locations.Where
                    (salvoLocation => ship.Locations.Any(shipLocation => shipLocation.Location == salvoLocation.Location))
                    .Select(salvoLocation => salvoLocation.Location).ToList()
                }).ToList()
            }).ToList();
        }
        public ICollection<string> getSunks()
        {
            //Declaro una variable llamada ultimo turno
            int lastTurn = Salvos.Count;
            //Lista del tipo string llamada salvoLocations
            List<string> salvoLocations =
                 //Obtengo los salvos del oponente cuando el turno de ese salvo  
                 //sea menor o igual al ultimo turno 
                 GetOpponet()?.Salvos
                .Where(salvo => salvo.Turn <= lastTurn)
                //SelectMany: para anidar listas
                .SelectMany(salvo => salvo.Locations.Select(location => location.Location)).ToList();
            return Ships?.Where(ship => ship.Locations.Select(shipLocation => shipLocation.Location)
            .All(shipLocation => shipLocation != null ? salvoLocations.Any(salvoLocations => salvoLocations == shipLocation) : false))
            .Select(ship => ship.Type).ToList();
        }

        /*-----------------------------------Actividad 11--------------------------*/
        public GameState GetGameState()
        {
            GameState gameState = GameState.ENTER_SALVO;

            //Evaluar los ships
            if (Ships == null || Ships?.Count == 0)
                gameState = GameState.PLACE_SHIPS;
            else if (GetOpponet() == null) //Si no hay oponente
            {
                //Los salvos son distinto de nulo y la cant de salvos disparados por el jugador que esta
                //autenticado es mayor a cero
                if (Salvos == null && Salvos?.Count > 0)
                    //Tendra que esperar
                    gameState = GameState.WAIT;
            }
            else
            {
                //Si hay un oponente
                GamePlayer opponent = GetOpponet();
                int turn = Salvos != null ? Salvos.Count() : 0;
                int opponentTurn = opponent.Salvos != null ? opponent.Salvos.Count() : 0;

                if (turn > opponentTurn)
                    gameState = GameState.WAIT;
                else if (turn == opponentTurn && turn != 0)
                {
                    int playerSunks = getSunks().Count();
                    int opponentSunks = opponent.getSunks().Count();

                    if (playerSunks == Ships.Count() && opponentSunks == opponent.Ships.Count())
                        gameState = GameState.TIE;
                    else if (playerSunks == Ships.Count())
                        gameState = GameState.LOSS;
                    else if (opponentSunks == opponent.Ships.Count())
                        gameState = GameState.WIN;
                }
            }

            return gameState;
        }

    }
}
