namespace EmpiriCall.Data.DataAccess
{
    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        TResult Handle(TQuery args);
    }
}