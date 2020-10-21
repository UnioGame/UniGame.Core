namespace UniModules.UniGame.Core.Runtime.Interfaces
{
    public interface IValidator<TData>
    {
        bool Validate(TData data);
    }

    public interface IValidator
    {
        bool Validate();
    }
}
