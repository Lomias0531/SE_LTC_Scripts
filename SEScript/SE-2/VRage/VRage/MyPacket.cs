using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;

namespace VRage
{
	public abstract class MyPacket
	{
		public BitStream BitStream;

		public ByteStream ByteStream;

		public Endpoint Sender;

		public MyTimeSpan ReceivedTime;

		public abstract void Return();
	}
}
