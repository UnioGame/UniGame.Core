namespace UniGame.Core.Runtime
{
    public interface IContextDataWriter<TContext>
    {
        void UpdateValue<TData>(TContext context, TData value);
        
        bool RemoveContext(TContext context);
        
        bool Remove<TData>(TContext context);
    }
}