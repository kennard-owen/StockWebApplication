using IRepository;
using SqlSugar;
using PF.Core.Entity;

namespace Repository
{
    public class Sys_menuRepository : BaseRepository<Sys_menu>, ISys_menuRepository
    {
        public Sys_menuRepository(SqlSugarClient dbContext) : base(dbContext)
        {
        }
    }
}