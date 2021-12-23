using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Salvo.Models;
using Salvo.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Salvo.Controllers
{
    [Route("api/gamePlayers")]
    [ApiController]
    [Authorize("PlayerOnly")]
    public class GamesPlayersController : ControllerBase
    {
        private IGamePlayerRepository _repository;
        private IPlayerRepository _playerRepository;
        private IScoreRepository _scoreRepository;//AGREGO DE LA ACTIVIDAD 11

        //llevan guin bajo las variables cuando son de largo alcance 
        public GamesPlayersController(IGamePlayerRepository repository, IPlayerRepository playerRepository, IScoreRepository scoreRepository)
        {
            _repository = repository;
            _playerRepository = playerRepository;
            _scoreRepository = scoreRepository;
        }
        // GET api/GamePlayers/5
        [HttpGet("{id}", Name = "GetGameView")]
        public IActionResult GetGameView(int id)
        {
            try
            {
                /********Primero busco el usuario autenticado y obtengo el mail********/
                //Del User me trae un Player y si es distinto de nulo trae el valor que en este
                //caso es el mail sino invitado
                string email = User.FindFirst("Player") != null ? User.FindFirst("Player").Value : "Guest";
                //Luego obtengo el GamePlayer
                var gp = _repository.GetGamePlayerView(id);
                //Verifico si el gp(gameplayer) corresponde al mismo email del usuario autenticado//
                /*Si el email del player es distinto del email del usuario autenticado 
                 retorno Forbid*/
                if (gp.Player.Email != email)
                    return Forbid(); //Prohibo la autenticacion
                /****************************************************************************/

                var gameView = new GameViewDTO
                {
                    Id = gp.Id,
                    CreationDate = gp.Game.CreationDate,
                    Ships = gp.Ships.Select(ship => new ShipDTO
                    {
                        Id = ship.Id,
                        Type = ship.Type,
                        Locations = ship.Locations.Select(shipLocation => new ShipLocationDTO
                        {
                            Id = shipLocation.Id,
                            Location = shipLocation.Location
                        }).ToList()
                    }).ToList(),
                    GamePlayers = gp.Game.GamePlayers.Select(gps => new GamePlayerDTO
                    {
                        Id = gps.Id,
                        JoinDate = gps.JoinDate,
                        Player = new PlayerDTO
                        {
                            Id = gps.Player.Id,
                            Email = gps.Player.Email
                        }
                    }).ToList(),
                    Salvos = gp.Game.GamePlayers.SelectMany(gps => gps.Salvos.Select(salvo => new SalvoDTO
                    {
                        Id = salvo.Id,
                        Turn = salvo.Turn,
                        Player = new PlayerDTO
                        {
                            Id = gps.Player.Id,
                            Email = gps.Player.Email
                        },
                        Locations = salvo.Locations.Select(salvoLocation => new SalvoLocationDTO
                        {
                            Id = salvoLocation.Id,
                            Location = salvoLocation.Location
                        }).ToList()
                    })).ToList(),
                    /*-----------------------------------Actividad 10--------------------------*/
                    Hits = gp.getHits(),
                    HitsOpponent = gp.GetOpponet()?.getHits(),
                    Sunks = gp.getSunks(),
                    SunksOpponent = gp.GetOpponet()?.getSunks(),
                    /*-------------------------------------------------------------------------*/
                    /*-----------------------------------Actividad 11--------------------------*/
                    GameState = Enum.GetName(typeof(GameState), gp.GetGameState())
                    /*-------------------------------------------------------------------------*/
                };
                return Ok(gameView);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);

            }
        }

        //Post 
        [HttpPost("{id}/ships")]
        public IActionResult Post(long id, [FromBody] List<ShipDTO> ships)
        {
            try
            {
                string email = User.FindFirst("Player") != null ? User.FindFirst("Player").Value : "Guest";
                Player player = _playerRepository.FindByEmail(email);
                GamePlayer gamePlayer = _repository.FindById(id);
                if (gamePlayer == null)
                    return StatusCode(403, "No existe el juego");
                //gamePlayer.Player.Email != email , cumpliria la misma funcion 
                //solo que lo hacemos de esta manera por el momento ya que mas 
                //adelante necesitare otros repositorios 
                if (gamePlayer.Player.Id != player.Id)
                    return StatusCode(403, "El usuario no se encuentra en el juego");
                if (gamePlayer.Ships.Count == 5)
                    return StatusCode(403, "Ya se han posicionado los barcos");

                //los ships son los que vienen por parametro
                gamePlayer.Ships = ships.Select(ship => new Ship
                {
                    GamePlayerId = gamePlayer.Id,
                    Type = ship.Type,
                    Locations = ship.Locations.Select(location => new ShipLocation
                    {
                        ShipId = ship.Id,
                        Location = location.Location
                    }).ToList()
                }).ToList();

                //Uso el reposotirio para guardar el gameplayer
                _repository.Save(gamePlayer);

                return StatusCode(201);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }

        }

        //Post 
        [HttpPost("{id}/salvos")]
        public IActionResult Post(long id, [FromBody] SalvoDTO salvo)
        {
            try
            {
                //Buscar el mail logueado
                string email = User.FindFirst("Player") != null ? User.FindFirst("Player").Value : "Guest";
                Player player = _playerRepository.FindByEmail(email);

                //Buscar el gamePlayer
                GamePlayer gamePlayer = _repository.FindById(id);//usuario que esta logueado o sea yo

                if (gamePlayer == null)
                    return StatusCode(403, "No existe el juego");
                if (gamePlayer.Player.Id != player.Id)
                    return StatusCode(403, "El usuario no se encuentra en el juego");

                /*------------------------Actividad 11---------------------------*/
                //Obtengo el gamestate
                GameState gameState = gamePlayer.GetGameState();

                if (gameState == GameState.LOSS || gameState == GameState.WIN || gameState == GameState.TIE)
                    return StatusCode(403, "El juego terminó");
                /*---------------------------------------------------------------*/

                //Buscar al (mi)oponente ya que arriba ya tengo al gamePlayer logueado(yo)
                GamePlayer opponentGamePlayer = gamePlayer.GetOpponet();//usuario logueado que no soy yo

                /*----------------Validaciones adicionales-----------------*/
                //Validaciones p/ saber si es que hay un oponente a quien disparar
                //if (opponentGamePlayer == null)
                //    return StatusCode(403, "No hay nadie para disparar!!!");
                //if(gamePlayer.Game.GamePlayers.Count()!= 2)
                //    return StatusCode(403, "No hay nadie para disparar!!!");
                if (gamePlayer.Game.GamePlayers.Count() < 2)
                    return StatusCode(403, "No hay nadie para disparar!!!");

                //Validaciones p/ saber si el oponente posiciono los barcos
                //opponentGamePlayer = _repository.FindById(opponentGamePlayer.Id);
                if (gamePlayer.Ships.Count() == 0)
                    return StatusCode(403, "El usuario logueado no ha posicionado los barcos");
                if (opponentGamePlayer.Ships.Count() == 0)
                    return StatusCode(403, "El oponente no ha posicionado los Barcos");



                int playerTurn = 0;//seria mi turno o sea que estoy autenticado
                int opponentTurn = 0;//seria el turno del opononte
                //Defino cual es el turno y de quien
                //si es distinto de 1 a la cantidad de salvos le sumo 1, es decir es primero mi turno
                //sino el turno mio sera el dos o el proximo
                //a medida que voy disparando se incrementa en 1 el turno
                playerTurn = gamePlayer.Salvos != null ? gamePlayer.Salvos.Count + 1 : 1;

                if (opponentGamePlayer != null)
                    opponentTurn = opponentGamePlayer.Salvos != null ? opponentGamePlayer.Salvos.Count() : 0;

                if ((playerTurn - opponentTurn) < -1 || (playerTurn - opponentTurn) > 1)
                    return StatusCode(403, "No se puede adelantar el turno");

                //Metodo par agregar el disparo
                //los salvo son los que vienen por parametro
                gamePlayer.Salvos.Add(new Salvo.Models.Salvo
                {
                    GamePlayerId = gamePlayer.Id,
                    Turn = playerTurn,
                    Locations = salvo.Locations.Select(location => new SalvoLocation
                    {
                        SalvoId = salvo.Id,
                        Location = location.Location
                    }).ToList()
                });

                //Uso el reposotirio para guardar el gameplayer
                _repository.Save(gamePlayer);

                /*-------------------------Actividad 11-----------------------*/
                //Procesos para guardar el score
                gameState = gamePlayer.GetGameState();

                if (gameState == GameState.WIN)
                {
                    Score score = new Score
                    {
                        FinishDate = DateTime.Now,
                        GameId = gamePlayer.GameId,
                        PlayerId = gamePlayer.PlayerId,
                        Point = 1
                    };
                    _scoreRepository.save(score);

                    Score scoreopponent = new Score
                    {
                        FinishDate = DateTime.Now,
                        GameId = gamePlayer.GameId,
                        PlayerId = opponentGamePlayer.PlayerId,
                        Point = 0
                    };
                    _scoreRepository.save(scoreopponent);
                }
                else if (gameState == GameState.LOSS)
                {
                    Score score = new Score
                    {
                        FinishDate = DateTime.Now,
                        GameId = gamePlayer.GameId,
                        PlayerId = gamePlayer.PlayerId,
                        Point = 0
                    };
                    _scoreRepository.save(score);

                    Score scoreopponent = new Score
                    {
                        FinishDate = DateTime.Now,
                        GameId = gamePlayer.GameId,
                        PlayerId = opponentGamePlayer.PlayerId,
                        Point = 1
                    };
                    _scoreRepository.save(scoreopponent);
                }
                else if (gameState == GameState.TIE)
                {
                    Score score = new Score
                    {
                        FinishDate = DateTime.Now,
                        GameId = gamePlayer.GameId,
                        PlayerId = gamePlayer.PlayerId,
                        Point = 0.5
                    };
                    _scoreRepository.save(score);

                    Score scoreopponent = new Score
                    {
                        FinishDate = DateTime.Now,
                        GameId = gamePlayer.GameId,
                        PlayerId = opponentGamePlayer.PlayerId,
                        Point = 0.5
                    };
                    _scoreRepository.save(scoreopponent);
                }

                return StatusCode(201);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }

        }

    }
}
