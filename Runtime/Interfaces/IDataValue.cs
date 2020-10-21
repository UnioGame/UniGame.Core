namespace UniModules.UniGame.Core.Runtime.Interfaces
{
	using Rx;

	public interface IDataValue<TData,TApi> : 
		IObservableValue<TApi>,
		IValueWriter<TData>
	{

	}
	
	public interface IDataValue<TData> : IDataValue<TData,TData>
	{

	}
}

