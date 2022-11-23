namespace UniGame.Core.Runtime
{
    public interface IMutableUnique : IUnique
    {
        void SetId(int id);
    }
}