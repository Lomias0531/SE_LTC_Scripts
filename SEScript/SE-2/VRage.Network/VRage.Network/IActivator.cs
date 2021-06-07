namespace VRage.Network
{
	public interface IActivator
	{
		object CreateInstance();
	}
	public interface IActivator<T> : IActivator
	{
		new T CreateInstance();
	}
}
