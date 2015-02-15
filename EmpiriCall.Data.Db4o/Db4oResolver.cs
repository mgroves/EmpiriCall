using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EmpiriCall.Data.Data;
using EmpiriCall.Data.DataAccess;
using EmpiriCall.Data.DataAccess.CommandQueries;
using EmpiriCall.Data.Db4o.CommandHandlers;
using EmpiriCall.Data.Db4o.QueryHandlers;

namespace EmpiriCall.Data.Db4o
{
    public class Db4oResolver : IDependencyResolver
    {
        readonly string _db4oFilePath;

        public Db4oResolver(string db4oFilePath)
        {
            _db4oFilePath = db4oFilePath;
        }

        public object GetService(Type serviceType)
        {
            // TODO: there needs to be a better way of doing this
            // but it's a pretty small and flat dependency graph
            // so I'm not worried about it yet
            if (serviceType == typeof (ICommandHandler<CommandAddNewMetaDataVersion>))
                return new CommandHandlerAddNewMetaDataVersion(_db4oFilePath);
            else if (serviceType == typeof (ICommandHandler<CommandAddRecord>))
                return new CommandHandlerAddRecord(_db4oFilePath);
            else if (serviceType == typeof (IQueryHandler<QueryRawDetail, List<DetailRecord>>))
                return new QueryHandlerRawDetail(_db4oFilePath);
            else if (serviceType == typeof (IQueryHandler<QueryGetLatestMetaData, MetaData>))
                return new QueryHandlerGetLatestMetaData(_db4oFilePath);
            else if (serviceType == typeof (ICommandHandler<CommandMapFeature>))
                return new CommandHandlerMapFeature(_db4oFilePath);
            else if (serviceType == typeof (ICommandHandler<CommandCreateMetaDataIfNecessary>))
                return new CommandHandlerCreateMetaDataIfNecessary(_db4oFilePath);
            else
                throw new Exception("I can't find a QueryHandler that returns: " + serviceType.FullName);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            // not needed by Processor
            throw new NotImplementedException();
        }
    }
}