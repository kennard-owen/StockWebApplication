using IRepository;
using SqlSugar;
using PF.Core.Entity;

namespace Repository
{
    public class FundRepository : BaseRepository<Conceptionfund>, IfundRepository {
        public FundRepository(SqlSugarClient dbContext) : base(dbContext)
        {
        }
    }
}