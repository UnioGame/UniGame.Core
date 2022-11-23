namespace UniGame.Core.Runtime
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
