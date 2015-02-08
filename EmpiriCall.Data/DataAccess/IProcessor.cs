namespace EmpiriCall.Data.DataAccess
{
    public interface IProcessor
    {
        TResult Query<TResult>(IQuery<TResult> query);
        void Execute(ICommand command);
    }
}