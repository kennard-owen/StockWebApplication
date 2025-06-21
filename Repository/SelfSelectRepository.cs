using PF.Core.Entity;
using IRepository;
using SqlSugar;

namespace Repository
{
    public class SelfSelectRepository : BaseRepository<SelfSelect>, ISelfSelectRepository {
        public SelfSelectRepository(SqlSugarClient dbContext) : base(dbContext)
        {
        }
    }
}