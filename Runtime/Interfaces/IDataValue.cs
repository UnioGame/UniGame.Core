namespace UniGame.Core.Runtime
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

