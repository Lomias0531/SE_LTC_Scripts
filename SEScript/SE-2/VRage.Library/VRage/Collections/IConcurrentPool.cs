namespace VRage.Collections
{
	internal interface IConcurrentPool
	{
		object Get();

		void Return(object obj);
	}
}
