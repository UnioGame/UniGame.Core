namespace UniModules.UniGame.Runtime
{
    public interface IId<out T> where T:struct
    {
        T FromString(string value);
    }
}