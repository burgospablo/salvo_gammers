using Salvo.Models;

namespace Salvo.Repositories
{
    public interface IGamePlayerRepository
    {
        GamePlayer GetGamePlayerView(long idGamePlayer);
        void Save(GamePlayer getGamePlayer);
        GamePlayer FindById(long id);
    }
}
