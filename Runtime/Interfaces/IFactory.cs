namespace UniGame.Core.Runtime
{
    public interface IFactory<out TProduct>
    {
        TProduct Create();
    }
    
    public interface IFactory<in TContext,out TProduct>
    {
        public TProduct Create(TContext data);
    }
    
}
