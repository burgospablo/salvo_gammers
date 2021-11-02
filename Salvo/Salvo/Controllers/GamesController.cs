using Microsoft.AspNetCore.Mvc;
using Salvo.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Salvo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        //Propiedad privada para ser usada solo en esta clase
        private IGameRepository _repository;

        //Constructor
        public GamesController(IGameRepository repository) 
        {
            _repository = repository;
        }
        //Nota: las lineas anteriores las necesito para poder implementar los get,put,delete,etc.

        // GET: api/<GamesController>
        [HttpGet]
        public IActionResult Get()
        {
            //Si se produce un error dentro del try y antes del return ok
            //devuelva un mensaje con ese error(dentro del catch)
            try
            {
                //devuelve los games
                var games = _repository.GetAllGames();
                return Ok(games);
            }
            catch (Exception ex) 
            {
                return StatusCode(500,ex.Message);
            }
            
        }

        
    }
}
