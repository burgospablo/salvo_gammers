using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Salvo.Models;
using Salvo.Repositories;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Salvo.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        //Siempre debo crear estos dos: una variable privada y un constructor
        //Propiedad privada para ser usada solo en esta clase
        private IPlayerRepository _repository;

        //Constructor
        public AuthController(IPlayerRepository repository)
        {
            _repository = repository;
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        //IActionResult es un parametro de una tarea(Task) asincrona(async) 
        //El nombre del metodo se llama login y va a recibir los parametros desdel body
        //y el parametro que va a recibir es una PlayerDTO y a ese parametro le llamo player
        public async Task<IActionResult> Login([FromBody] PlayerDTO player)
        {
            try
            {
                //Nombre de la variable user y es del tipo player
                //va a ser igual a repository con el metodo Find le paso el mail
                //finalmente me va a retornar un player o un nulo
                Player user = _repository.FindByEmail(player.Email);
                //Si el usuario es nulo significa que no lo encontro o en el caso
                //de que si lo encuentre y la password de ese usuario es distinta de la
                //password que esta en el parametro player de PlayerDTO
                //entonces no esta autorizado para entrar. 
                if (user == null || !String.Equals(user.Password, player.Password))
                    return Unauthorized();

                var claims = new List<Claim>
                    {
                        new Claim("Player", user.Email)
                    };
                var claimsIdentity = new ClaimsIdentity(
                    //recibe como paramentro la lista claims y lo segundo define el esquema de autenticacion
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme
                    );

                await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));

                return Ok();// puede retornar algo
            }
            catch (Exception ex)
            {
                //Retorne Error 500: error de servidor porque algo paso
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/Auth/logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
