using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Salvo.Models;
using Salvo.Repositories;
using System;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Salvo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]//Lo autoriza sin politica
    public class GamesController : ControllerBase
    {
        //Propiedad privada para ser usada solo en esta clase
        private IGameRepository _repository;
        private IPlayerRepository _playerRepository;
        private IGamePlayerRepository _gamePlayerRepository;

        //Constructor
        public GamesController(IGameRepository repository,
            IPlayerRepository playerRepository, IGamePlayerRepository gamePlayerRepository)
        {
            _repository = repository;
            _playerRepository = playerRepository;
            _gamePlayerRepository = gamePlayerRepository;
        }
        //Nota: las lineas anteriores las necesito para poder implementar los get,put,delete,etc.

        // GET: api/Games
        [HttpGet]
        [AllowAnonymous]//Permite que cualquiera lo vea por mas que no este autenticado
        public IActionResult Get()
        {
            //Si se produce un error dentro del try y antes del return ok
            //devuelva un mensaje con ese error(dentro del catch)
            try
            {
                //devuelve los games
                //var games = _repository.GetAllGames();
                //var games = _repository.GetAllGamesWithPlayers();

                GameListDTO gameList = new GameListDTO
                {
                    Email = User.FindFirst("Player") != null ? User.FindFirst("Player").Value : "Guest",
                    Games = _repository.GetAllGamesWithPlayers()
                    .Select(g => new GameDTO
                    {
                        Id = g.Id,
                        CreationDate = g.CreationDate,
                        GamePlayers = g.GamePlayers.Select(gp => new GamePlayerDTO
                        {
                            Id = gp.Id,
                            JoinDate = gp.JoinDate,
                            Player = new PlayerDTO
                            {
                                Id = gp.Player.Id,
                                Email = gp.Player.Email
                            },
                            //Operacion ternaria:
                            //Si es distinto de nulo entonces(?) devuelve este valor (double?)gp.GetScore().Point
                            //Si es nulo devuelva lo que viene despues de los dos puntos null
                            //(double?) hace cast que es del tipo double y que es nuliable 
                            //Si le pongo 0 se interpreta como que la partida terminor, por eso pongo null
                            Point = gp.GetScore() != null ? (double?)gp.GetScore().Point : null
                        }).ToList()
                    }).ToList()
                };
                return Ok(gameList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        // GET: api/Games
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Games
        [HttpPost]
        public IActionResult Post()
        {
            try
            {
                //Del User me trae un Player y si es distinto de nulo trae el valor que en este
                //caso es el mail sino invitado
                string email = User.FindFirst("Player") != null ? User.FindFirst("Player").Value : "Guest";
                //Busco al jugador autenticado: aprovecho el _playerRepository 
                //y con FindByEmail trae el email de ese jugador 
                Player player = _playerRepository.FindByEmail(email);
                DateTime fechaActual = DateTime.Now; //Con esta forma se crea exactamente igual la fecha dia y hora
                //Creo un GamePlayer y a esta variable la llamo gamePlayer y va sin identificador
                GamePlayer gamePlayer = new GamePlayer

                {
                    ///******Lo que contiene el GamePlayer*******/
                    ////Creo un Game
                    //Game = new Game
                    //{
                    //    //Fecha de Creacion
                    //    CreationDate = DateTime.Now
                    //},
                    ////Obtengo el Id del player
                    //PlayerId = player.Id,
                    ////obtengo fecha y hora actual
                    //JoinDate= DateTime.Now
                    //EN ESTE CASO SE CREAN CON MILISEGUNDOS DE DIFERENCIAS

                    //Otra forma
                    /******Lo que contiene el GamePlayer*******/
                    //Creo un Game que como no tiene Id solo los que tiene a continuacion
                    Game = new Game
                    {
                        //Fecha de Creacion
                        CreationDate = fechaActual
                    },
                    //Obtengo el Id del player
                    PlayerId = player.Id,
                    //obtengo fecha y hora actual
                    JoinDate = fechaActual
                    //Con esta forma se crea exactamente igual la fecha dia y hora
                };
                //Guardo el GamePlayer desde el repositorio 
                _gamePlayerRepository.Save(gamePlayer);
                //Retorna un StatusCode 201 con el Id del gamePlayer recien creado
                return StatusCode(201, gamePlayer.Id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /********************Segunda parte de la actividad 7 ************************/
        //Creo un nuevo metodo POST
        // POST: api/Games
        [HttpPost("{id}/players", Name = "Join")]//Recibi un id de players y le llamo Join
        public IActionResult Join(long id)
        {
            try
            {
                //Del User me trae un Player y si es distinto de nulo trae el valor 
                //que en este caso es el mail sino invitado
                string email = User.FindFirst("Player") != null ? User.FindFirst("Player").Value : "Guest";
                //Busco al jugador autenticado: aprovecho el _playerRepository 
                //y con FindByEmail trae el email de ese jugador 
                Player player = _playerRepository.FindByEmail(email);
                //Buscamos nuestro game
                Game game = _repository.FindById(id);
                //Validacion 1
                if (game == null)//existe el juego
                    return StatusCode(403, "No existe el juego");
                //Validacion 2
                //Busco dentro de los GamePlayers de este game(juego) todos los player.Id
                //que corresponden a los usuarios autenticados y que debería uno solo
                //si esta condicion retorna un nulo quiere decir que ese jugador no esta en ese juego
                //sin embargo si retorna distinto de nulo quiere decir que el jugador ya esta dentro del juego
                if (game.GamePlayers.Where(gp => gp.Player.Id == player.Id).FirstOrDefault() != null)
                    return StatusCode(403, "Ya se encuentra el jugador en el juego");
                //Validacion 3
                //Si dentro de un juego hay mas de un gameplayer quiere decir que el juego esta lleno
                if (game.GamePlayers.Count > 1)//existe el juego
                    return StatusCode(403, "Juego lleno");

                //Creo un nuevo game Player
                GamePlayer gamePlayer = new GamePlayer
                {
                    GameId = game.Id,
                    PlayerId = player.Id,
                    JoinDate = DateTime.Now
                };
                //Guardo en la base de datos
                _gamePlayerRepository.Save(gamePlayer);

                //retornamos
                return StatusCode(201, gamePlayer.Id);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }

    }
}
