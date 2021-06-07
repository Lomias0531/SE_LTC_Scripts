using VRage;

namespace Sandbox.Engine.Multiplayer
{
	internal interface IControlMessageCallback<TMsg> : ITransportCallback
	{
		void Write(ByteStream destination, ref TMsg msg);
	}
}
