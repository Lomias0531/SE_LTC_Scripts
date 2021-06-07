namespace VRage.Network
{
	public interface ICallSite
	{
	}
	public interface ICallSite<T1, T2, T3, T4, T5, T6, T7> : ICallSite
	{
		void Invoke(in T1 arg1, in T2 arg2, in T3 arg3, in T4 arg4, in T5 arg5, in T6 arg6, in T7 arg7);
	}
}
