using System;
using System.Web.Mvc;

namespace EmpiriCall.Data.DataAccess
{
    public class Processor : IProcessor
    {
        readonly IDependencyResolver _resolver;

        public Processor(IDependencyResolver resolver)
        {
            _resolver = resolver;
        }

        public TResult Query<TResult>(IQuery<TResult> query)
        {
            var handlerType = typeof(IQueryHandler<,>)
                .MakeGenericType(query.GetType(), typeof(TResult));

            dynamic handler = _resolver.GetService(handlerType);

            if(handler == null)
                throw new Exception("No service found for: " + query.GetType().Name);

            return handler.Handle((dynamic)query);
        }

        public void Execute(ICommand command)
        {
            var handlerType = typeof(ICommandHandler<>)
                .MakeGenericType(command.GetType());

            dynamic handler = _resolver.GetService(handlerType);

            handler.Handle((dynamic)command);
        }
    }
}