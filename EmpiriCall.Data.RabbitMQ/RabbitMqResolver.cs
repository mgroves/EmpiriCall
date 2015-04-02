using System;
using System.Collections.Generic;
using System.Data.Common;
using EmpiriCall.Data.Data;
using EmpiriCall.Data.DataAccess;
using EmpiriCall.Data.DataAccess.CommandQueries;
using EmpiriCall.Data.RabbitMQ.CommandHandlers;
using EmpiriCall.Data.RabbitMQ.QueryHandlers;
using System.Web.Mvc;

namespace EmpiriCall.Data.RabbitMQ
{
    public class RabbitMqResolver : IDependencyResolver
    {
        readonly EmpiriCallDbContext _context;
        readonly string _rabbitMqHostName;
        readonly string _rabbitMqQueueName;

        public RabbitMqResolver(DbConnection connectionString, string rabbitMqHostName, string rabbitMqQueueName = "EmpiriCallRawRecord")
        {
            _context = new EmpiriCallDbContext(connectionString);
            _rabbitMqHostName = rabbitMqHostName;
            _rabbitMqQueueName = rabbitMqQueueName;
        }

        public object GetService(Type serviceType)
        {
            // TODO: there needs to be a better way of doing this
            // but it's a pretty small and flat dependency graph
            // so I'm not worried about it yet
            if (serviceType == typeof(ICommandHandler<CommandAddNewMetaDataVersion>))
                return new CommandHandlerAddNewMetaDataVersion(_context);
            else if (serviceType == typeof(ICommandHandler<CommandAddRecord>))
                return new CommandHandlerAddRecord(_rabbitMqHostName, _rabbitMqQueueName);
            else if (serviceType == typeof(IQueryHandler<QueryRawDetail, List<DetailRecord>>))
                return new QueryHandlerRawDetail(_context);
            else if (serviceType == typeof(IQueryHandler<QueryGetLatestMetaData, MetaData>))
                return new QueryHandlerGetLatestMetaData(_context);
            else if (serviceType == typeof(ICommandHandler<CommandMapFeature>))
                return new CommandHandlerMapFeature(_context);
            else if (serviceType == typeof(ICommandHandler<CommandCreateMetaDataIfNecessary>))
                return new CommandHandlerCreateMetaDataIfNecessary(_context);
            else
                throw new Exception("I can't find a QueryHandler that returns: " + serviceType.FullName);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            throw new NotImplementedException();
        }
    }
}