using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Web.Mvc;
using EmpiriCall.Data.Data;
using EmpiriCall.Data.DataAccess;
using EmpiriCall.Data.DataAccess.CommandQueries;
using EmpiriCall.Data.SQLServer.CommandHandlers;
using EmpiriCall.Data.SQLServer.QueryHandlers;

namespace EmpiriCall.Data.SQLServer
{
    public class SqlServerResolver : IDependencyResolver
    {
        readonly EmpiriCallDbContext _context;

        public SqlServerResolver(DbConnection connectionString)
        {
            _context = new EmpiriCallDbContext(connectionString);
        }

        public object GetService(Type serviceType)
        {
            // TODO: there needs to be a better way of doing this
            // but it's a pretty small and flat dependency graph
            // so I'm not worried about it yet
            if (serviceType == typeof(ICommandHandler<CommandMetaDataUpdate>))
                return new CommandHandlerMetaDataUpdate(_context);
            else if (serviceType == typeof(ICommandHandler<CommandAddRecord>))
                return new CommandHandlerAddRecord(_context);
            else if (serviceType == typeof(IQueryHandler<QueryRawDetail, List<DetailRecord>>))
                return new QueryHandlerRawDetail(_context);
            else if (serviceType == typeof(IQueryHandler<QueryGetMetaData, MetaData>))
                return new QueryHandlerGetMetaData(_context);
            else if (serviceType == typeof(ICommandHandler<CommandMapFeature>))
                return new CommandHandlerMapFeature(_context);
            else
                throw new Exception("I can't find a QueryHandler that returns: " + serviceType.FullName);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            throw new NotImplementedException();
        }
    }
}