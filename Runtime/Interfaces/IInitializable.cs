namespace UniModules.UniGame.Core.Runtime.Interfaces
{
	public interface IInitializable<TContext>
	{

		void Initialize(TContext context);
	
	}
}
