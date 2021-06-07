using VRage;

namespace Sandbox.Engine.Multiplayer
{
	public interface ITransportCallback
	{
		void Receive(ByteStream source, ulong sender);
	}
}
