using Salvo.Models;
/*----------------------Actividad 11---------------------------------*/
namespace Salvo.Repositories
{
    public class ScoreRepository : RepositoryBase<Score>, IScoreRepository
    {
        public ScoreRepository(SalvoContext repositoryContext) : base(repositoryContext) { }
        public void save(Score score)
        {
            Create(score);
            SaveChanges();
        }
    }
}
