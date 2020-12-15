namespace UniModules.UniGame.Core.Runtime.Interfaces
{
    public interface IRollbackCommand : ICommand
    {
        bool Rollback();
    }

}