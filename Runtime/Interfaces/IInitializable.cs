namespace UniGame.Core.Runtime
{
	public interface IInitializable<TContext>
	{

		void Initialize(TContext context);
	
	}
}
