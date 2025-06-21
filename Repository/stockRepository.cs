using IRepository;
using SqlSugar;
using PF.Core.Entity;

namespace Repository
{
    public class stockRepository : BaseRepository<Stock>, IstockRepository
    {
        public stockRepository(SqlSugarClient dbContext) : base(dbContext)
        {
        }
    }
}