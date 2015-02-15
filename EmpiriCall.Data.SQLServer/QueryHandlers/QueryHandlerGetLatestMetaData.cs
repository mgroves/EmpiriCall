using System.Linq;
using EmpiriCall.Data.Data;
using EmpiriCall.Data.DataAccess;
using EmpiriCall.Data.DataAccess.CommandQueries;

namespace EmpiriCall.Data.SQLServer.QueryHandlers
{
    public class QueryHandlerGetLatestMetaData : IQueryHandler<QueryGetLatestMetaData, MetaData>
    {
        readonly EmpiriCallDbContext _context;

        public QueryHandlerGetLatestMetaData(EmpiriCallDbContext context)
        {
            _context = context;
        }

        public MetaData Handle(QueryGetLatestMetaData args)
        {
            return _context.MetaData.OrderByDescending(m => m.Version).FirstOrDefault();
        }
    }
}