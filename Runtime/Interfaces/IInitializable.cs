namespace UniModules.UniCore.Runtime.Interfaces
{
	public interface IInitializable<TContext>
	{

		void Initialize(TContext context);
	
	}
}
