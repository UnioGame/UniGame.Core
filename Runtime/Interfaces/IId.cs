namespace UniModules.UniGame.Runtime
{
    public interface IId<T> where T:struct
    {
        T FromString(string value);
    }
}