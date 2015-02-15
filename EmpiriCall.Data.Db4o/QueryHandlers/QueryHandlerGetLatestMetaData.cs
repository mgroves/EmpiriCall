using System.Linq;
using Db4objects.Db4o;
using EmpiriCall.Data.Data;
using EmpiriCall.Data.DataAccess;
using EmpiriCall.Data.DataAccess.CommandQueries;

namespace EmpiriCall.Data.Db4o.QueryHandlers
{
    public class QueryHandlerGetLatestMetaData : IQueryHandler<QueryGetLatestMetaData, MetaData>
    {
        readonly string _db4oFilePath;

        public QueryHandlerGetLatestMetaData(string db4oFilePath)
        {
            _db4oFilePath = db4oFilePath;
        }

        public MetaData Handle(QueryGetLatestMetaData args)
        {
            using (var db = Db4oEmbedded.OpenFile(_db4oFilePath))
                return db.Query<MetaData>().OrderByDescending(m => m.Version).FirstOrDefault();
        }
    }
}