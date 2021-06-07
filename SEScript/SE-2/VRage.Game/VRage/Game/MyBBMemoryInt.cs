using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	[XmlType("MyBBMemoryInt")]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyBBMemoryInt : MyBBMemoryValue
	{
		protected class VRage_Game_MyBBMemoryInt_003C_003EIntValue_003C_003EAccessor : IMemberAccessor<MyBBMemoryInt, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyBBMemoryInt owner, in int value)
			{
				owner.IntValue = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyBBMemoryInt owner, out int value)
			{
				value = owner.IntValue;
			}
		}

		private class VRage_Game_MyBBMemoryInt_003C_003EActor : IActivator, IActivator<MyBBMemoryInt>
		{
			private sealed override object CreateInstance()
			{
				return new MyBBMemoryInt();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyBBMemoryInt CreateInstance()
			{
				return new MyBBMemoryInt();
			}

			MyBBMemoryInt IActivator<MyBBMemoryInt>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public int IntValue;
	}
}
