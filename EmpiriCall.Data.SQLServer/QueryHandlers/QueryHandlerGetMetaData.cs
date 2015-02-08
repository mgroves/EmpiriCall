using System.Linq;
using EmpiriCall.Data.Data;
using EmpiriCall.Data.DataAccess;
using EmpiriCall.Data.DataAccess.CommandQueries;

namespace EmpiriCall.Data.SQLServer.QueryHandlers
{
    public class QueryHandlerGetMetaData : IQueryHandler<QueryGetMetaData, MetaData>
    {
        readonly EmpiriCallDbContext _context;

        public QueryHandlerGetMetaData(EmpiriCallDbContext context)
        {
            _context = context;
        }

        public MetaData Handle(QueryGetMetaData args)
        {
            return _context.MetaData.SingleOrDefault();
        }
    }
}