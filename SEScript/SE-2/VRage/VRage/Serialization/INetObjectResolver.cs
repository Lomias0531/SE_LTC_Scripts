using VRage.Library.Collections;
using VRage.Network;

namespace VRage.Serialization
{
	public interface INetObjectResolver
	{
		void Resolve<T>(BitStream stream, ref T obj) where T : class, IMyNetObject;
	}
}
