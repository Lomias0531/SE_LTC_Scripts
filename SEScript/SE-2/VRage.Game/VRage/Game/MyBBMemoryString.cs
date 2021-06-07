using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	[XmlType("MyBBMemoryString")]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyBBMemoryString : MyBBMemoryValue
	{
		protected class VRage_Game_MyBBMemoryString_003C_003EStringValue_003C_003EAccessor : IMemberAccessor<MyBBMemoryString, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyBBMemoryString owner, in string value)
			{
				owner.StringValue = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyBBMemoryString owner, out string value)
			{
				value = owner.StringValue;
			}
		}

		private class VRage_Game_MyBBMemoryString_003C_003EActor : IActivator, IActivator<MyBBMemoryString>
		{
			private sealed override object CreateInstance()
			{
				return new MyBBMemoryString();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyBBMemoryString CreateInstance()
			{
				return new MyBBMemoryString();
			}

			MyBBMemoryString IActivator<MyBBMemoryString>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public string StringValue;
	}
}
