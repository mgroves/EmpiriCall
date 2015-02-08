using System.Collections.Generic;
using System.Linq;
using Db4objects.Db4o;
using EmpiriCall.Data.Data;
using EmpiriCall.Data.DataAccess;
using EmpiriCall.Data.DataAccess.CommandQueries;

namespace EmpiriCall.Data.Db4o.QueryHandlers
{
    public class QueryHandlerRawDetail : IQueryHandler<QueryRawDetail, List<DetailRecord>>
    {
        readonly string _db4oFilePath;

        public QueryHandlerRawDetail(string db4oFilePath)
        {
            _db4oFilePath = db4oFilePath;
        }

        public List<DetailRecord> Handle(QueryRawDetail args)
        {
            using (var db = Db4oEmbedded.OpenFile(_db4oFilePath))
                return db.Query<MetaData>().First().ActionInfo.SelectMany(x => x.CallRecords).ToList();
        }
    }
}