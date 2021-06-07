using System;
using VRage.Library.Collections;
using VRage.Network;
using VRage.Serialization;

namespace Sandbox.Engine.Multiplayer
{
	public class MyDynamicObjectResolver : IDynamicResolver
	{
		public void Serialize(BitStream stream, Type baseType, ref Type obj)
		{
			if (stream.Reading)
			{
				TypeId id = new TypeId(stream.ReadUInt32());
				obj = MyMultiplayer.Static.ReplicationLayer.GetType(id);
			}
			else
			{
				TypeId typeId = MyMultiplayer.Static.ReplicationLayer.GetTypeId(obj);
				stream.WriteUInt32(typeId);
			}
		}
	}
}
