using Salvo.Models;
using System.Linq;

namespace Salvo.Repositories
{
    public class PlayerRepository : RepositoryBase<Player>, IPlayerRepository
    {
        //La creacion de repositorio como clase me obliga
        //a crear dos cosas:un constructor e implementar la interfaz

        //CONSTRUCTOR
        public PlayerRepository(SalvoContext repositoryContext) : base(repositoryContext)
        {

        }
        //INTERFAZ
        public Player FindByEmail(string email)
        {
            //retorna un FindByCondition que recibe como expesion una consulta 
            //esta es una expresion lambda y dentro viene una condicion
            //predicado:player y de player el Email sea igual al email que viene por parametro
            //Como no retorno una lista sino un player, le digo que me retorne el primero o
            //el por defecto con el FirstOrDefault() que es un metodo de linq
            return FindByCondition(player => player.Email == email).FirstOrDefault();
        }

        public void Save(Player player)
        {
            //Como en respositorio base(RepositoryBase) ya cuento con un metodo para crear 
            //que se llama Create el cual recibe una clase cualquiera, entonces utilizo el mismo
            //metodo, le paso la clase player para que cree ese registro. 
            Create(player);//Crea el player 

            SaveChanges();//Lo guarda en la base de datos
        }
    }
}
