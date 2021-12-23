using Microsoft.AspNetCore.Mvc;
using Salvo.Models;
using Salvo.Repositories;
using System;
using System.Text.RegularExpressions;

namespace Salvo.Controllers
{
    [Route("api/players")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private IPlayerRepository _repository;
        //Constructor
        public PlayersController(IPlayerRepository repository)
        {
            ;
            _repository = repository;
        }

        //Metodo post
        [HttpPost]
        public IActionResult Post([FromBody] PlayerDTO player)
        {
            try
            {
                Regex regexEmail = new Regex(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z");
                Regex regexPassword = new Regex("^.*(?=.{10,})(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+=]).*$");
                if (!regexEmail.IsMatch(player.Email))
                {
                    return StatusCode(403, "Email inválido");
                }

                if (!regexPassword.IsMatch(player.Password))
                {
                    return StatusCode(403, "Contraseña inválida");
                }

                //Forma 1: como dice la guia 
                //Verifico que el email y la password no esten vacio
                if (String.IsNullOrEmpty(player.Email) || String.IsNullOrEmpty(player.Password))
                {
                    return StatusCode(403, "Datos inválidos");
                }

                //Debo no crear un usuario en el caso de que ya exista
                //Me retorna un nulo en el caso de que no exista y en el caso
                //de que exista me va a retornar un player 
                Player dbPlayer = _repository.FindByEmail(player.Email);
                if (dbPlayer != null)
                    return StatusCode(403, "Email esta en uso");
                //Si ocurre lo anterior que el email y la contraseña no son vacias
                //y el usuario no existe, entonces recien lo creo

                //NUEVO REGISTRO CON DOS ELEMENTOS--CREE EL NUEVO PLAYER
                //a partir del PlayerDTO
                Player newPlayer = new Player
                {
                    Email = player.Email,
                    Password = player.Password,
                    Name = player.Name
                };
                _repository.Save(newPlayer);//guardo el nuevo player
                //retorno el nuevo player(jugador)
                return StatusCode(201, newPlayer);


                ////Forma 2
                ////Verifico que el email y la password no esten vacio
                ////agregar una validacion de seguridad respecto a la clave
                ////cantidad de caracteres,mayusculas,numeros
                ////verificar estandar de claves
                ////pueden revisar expresiones regulares
                //    if (String.IsNullOrEmpty(player.Email))
                //        return StatusCode(403, "Email Vacio");
                //    if (String.IsNullOrEmpty(player.Password))
                //        return StatusCode(403, "Password Vacia");

                //return Ok();
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }

    }
}
