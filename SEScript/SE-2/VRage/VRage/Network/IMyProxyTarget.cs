namespace VRage.Network
{
	public interface IMyProxyTarget : IMyNetObject, IMyEventOwner
	{
		/// <summary>
		/// Gets target object.
		/// </summary>
		IMyEventProxy Target
		{
			get;
		}
	}
}
