using Sandbox.Game.Entities;
using VRage;
using VRage.Game.Entity;
using VRage.Serialization;

namespace Sandbox.Game.Multiplayer
{
	internal class MyEntitySerializer : ISerializer<MyEntity>
	{
		public static readonly MyEntitySerializer Default = new MyEntitySerializer();

		void ISerializer<MyEntity>.Serialize(ByteStream destination, ref MyEntity data)
		{
			long data2 = data.EntityId;
			BlitSerializer<long>.Default.Serialize(destination, ref data2);
		}

		void ISerializer<MyEntity>.Deserialize(ByteStream source, out MyEntity data)
		{
			BlitSerializer<long>.Default.Deserialize(source, out long data2);
			MyEntities.TryGetEntityById(data2, out data);
		}
	}
}
