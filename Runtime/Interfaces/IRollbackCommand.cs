namespace UniModules.UniCore.Runtime.Interfaces
{
    public interface IRollbackCommand : ICommand
    {
        bool Rollback();
    }
}