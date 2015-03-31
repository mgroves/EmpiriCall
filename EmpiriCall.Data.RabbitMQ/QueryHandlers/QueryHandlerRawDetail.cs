using System.Collections.Generic;
using System.Linq;
using EmpiriCall.Data.Data;
using EmpiriCall.Data.DataAccess;
using EmpiriCall.Data.DataAccess.CommandQueries;

namespace EmpiriCall.Data.RabbitMQ.QueryHandlers
{
    public class QueryHandlerRawDetail : IQueryHandler<QueryRawDetail, List<DetailRecord>>
    {
        readonly EmpiriCallDbContext _context;

        public QueryHandlerRawDetail(EmpiriCallDbContext context)
        {
            _context = context;
        }

        public List<DetailRecord> Handle(QueryRawDetail args)
        {
            return _context.DetailRecord.OrderByDescending(r => r.TimeStamp)
                .ThenBy(x => x.ActionInfo.ControllerName)
                .ThenBy(x => x.ActionInfo.ActionName)
                .ToList();
        }
    }
}