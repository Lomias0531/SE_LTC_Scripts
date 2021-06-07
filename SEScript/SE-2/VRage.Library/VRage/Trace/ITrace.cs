namespace VRage.Trace
{
	public interface ITrace
	{
		void Watch(string name, object value);

		void Send(string msg, string comment = null);
	}
}
