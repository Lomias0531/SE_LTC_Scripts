using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	[XmlType("MyBBMemoryLong")]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyBBMemoryLong : MyBBMemoryValue
	{
		protected class VRage_Game_MyBBMemoryLong_003C_003ELongValue_003C_003EAccessor : IMemberAccessor<MyBBMemoryLong, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyBBMemoryLong owner, in long value)
			{
				owner.LongValue = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyBBMemoryLong owner, out long value)
			{
				value = owner.LongValue;
			}
		}

		private class VRage_Game_MyBBMemoryLong_003C_003EActor : IActivator, IActivator<MyBBMemoryLong>
		{
			private sealed override object CreateInstance()
			{
				return new MyBBMemoryLong();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyBBMemoryLong CreateInstance()
			{
				return new MyBBMemoryLong();
			}

			MyBBMemoryLong IActivator<MyBBMemoryLong>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public long LongValue;
	}
}
