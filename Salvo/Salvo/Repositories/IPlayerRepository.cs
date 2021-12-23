using Salvo.Models;

namespace Salvo.Repositories
{
    //Hago la interfaz publica 
    public interface IPlayerRepository
    {
        //Metodo que me crea un player y uso un FindByEmail, si tuviera que devolver
        //varios player uso un FindAll y le pasaria algun include
        Player FindByEmail(string email);

        //Creo un metodo de guardado y no retorna nada con lo cual es un void
        void Save(Player player); //Recibe como parametro el Player y lo llamo player

    }
}
