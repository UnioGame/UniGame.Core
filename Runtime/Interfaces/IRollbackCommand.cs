namespace UniGame.Core.Runtime
{
    public interface IRollbackCommand : ICommand
    {
        bool Rollback();
    }

}