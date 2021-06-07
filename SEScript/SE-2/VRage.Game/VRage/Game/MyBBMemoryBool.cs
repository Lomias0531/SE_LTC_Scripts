using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	[XmlType("MyBBMemoryBool")]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyBBMemoryBool : MyBBMemoryValue
	{
		protected class VRage_Game_MyBBMemoryBool_003C_003EBoolValue_003C_003EAccessor : IMemberAccessor<MyBBMemoryBool, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyBBMemoryBool owner, in bool value)
			{
				owner.BoolValue = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyBBMemoryBool owner, out bool value)
			{
				value = owner.BoolValue;
			}
		}

		private class VRage_Game_MyBBMemoryBool_003C_003EActor : IActivator, IActivator<MyBBMemoryBool>
		{
			private sealed override object CreateInstance()
			{
				return new MyBBMemoryBool();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyBBMemoryBool CreateInstance()
			{
				return new MyBBMemoryBool();
			}

			MyBBMemoryBool IActivator<MyBBMemoryBool>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public bool BoolValue;
	}
}
