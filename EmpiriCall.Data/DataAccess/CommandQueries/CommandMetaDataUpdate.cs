namespace EmpiriCall.Data.DataAccess.CommandQueries
{
    public class CommandMetaDataUpdate : ICommand
    {
        public bool ForceUpdate { get; set; }
    }
}